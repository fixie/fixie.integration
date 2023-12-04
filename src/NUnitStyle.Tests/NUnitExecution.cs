namespace NUnitStyle.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fixie;
using Fixie.Integration;

public class NUnitExecution : IExecution
{
    public async Task Run(TestSuite testSuite)
    {
        foreach (var testClass in testSuite.TestClasses)
        {
            var instance = testClass.Construct();

            await Call<TestFixtureSetUpAttribute>(instance);

            foreach (var test in testClass.Tests)
            {
                await test.Start();

                if (test.HasParameters)
                {
                    foreach (var parameters in GetParameters(test))
                        await RunTestCaseLifecycle(instance, test, parameters);
                }
                else
                {
                    await RunTestCaseLifecycle(instance, test);
                }
            }

            await Call<TestFixtureTearDownAttribute>(instance);

            await instance.DisposeWhenApplicable();
        }
    }

    static async Task RunTestCaseLifecycle(object instance, Test test, params object[] parameters)
    {
        await Call<SetUpAttribute>(instance);

        try
        {
            await test.Method.Call(instance, parameters);

            if (test.Has<ExpectedExceptionAttribute>(out var attribute))
            {
                try
                {
                    throw new Exception("Expected exception of type " + attribute.ExpectedException + ".");
                }
                catch (Exception failureException)
                {
                    await test.Fail(parameters, failureException);
                }
            }
            else
            {
                await test.Pass(parameters);
            }
        }
        catch (Exception testMethodException)
        {
            if (test.Has<ExpectedExceptionAttribute>(out var attribute))
            {
                try
                {
                    if (!attribute.ExpectedException.IsInstanceOfType(testMethodException))
                    {
                        throw new Exception(
                            "Expected exception of type " + attribute.ExpectedException + " but an exception of type " +
                            testMethodException.GetType() + " was thrown.", testMethodException);
                    }

                    if (attribute.ExpectedMessage != null && testMethodException.Message != attribute.ExpectedMessage)
                    {
                        throw new Exception(
                            "Expected exception message '" + attribute.ExpectedMessage + "'" + " but was '" + testMethodException.Message + "'.",
                            testMethodException);
                    }

                    await test.Pass(parameters);
                }
                catch (Exception failureException)
                {
                    await test.Fail(parameters, failureException);
                }
            }
            else
            {
                await test.Fail(parameters, testMethodException);
            }
        }

        await Call<TearDownAttribute>(instance);
    }

    static async Task Call<TAttribute>(object instance) where TAttribute : Attribute
    {
        var query = instance.GetType().GetMethods().Where(x => ReflectionExtensions.Has<TAttribute>(x));

        foreach (var q in query)
            await q.Call(instance);
    }

    static IEnumerable<object[]> GetParameters(Test test)
    {
        var testInvocations = new List<object[]>();

        var testCaseSourceAttributes = test.GetAll<TestCaseSourceAttribute>();

        foreach (var attribute in testCaseSourceAttributes)
        {
            var sourceType = attribute.SourceType ?? test.Method.DeclaringType;

            if (sourceType == null)
                throw new Exception("Could not find source type for method " + test.Method.Name);

            var members = sourceType.GetMember(attribute.SourceName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (members.Length != 1)
                throw new Exception($"Found {members.Length} members named '{attribute.SourceName}' on type {sourceType}");

            var member = members.Single();

            testInvocations.AddRange(InvocationsForTestCaseSource(member));
        }

        return testInvocations;
    }

    static IEnumerable<object[]> InvocationsForTestCaseSource(MemberInfo member)
    {
        var field = member as FieldInfo;
        if (field != null && field.IsStatic)
            return (IEnumerable<object[]>)field.GetValue(null);

        var property = member as PropertyInfo;
        if (property != null && property.GetGetMethod(true).IsStatic)
            return (IEnumerable<object[]>)property.GetValue(null, null);

        var m = member as MethodInfo;
        if (m != null && m.IsStatic)
            return (IEnumerable<object[]>)m.Invoke(null, null);

        throw new Exception($"Member '{member.Name}' must be static to be used with TestCaseSource");
    }
}