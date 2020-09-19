namespace NUnitStyle.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        public IEnumerable<Type> TestClasses(IEnumerable<Type> concreteClasses)
            => concreteClasses.Where(x => x.Has<TestFixture>());

        public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
            => publicMethods
                .Where(x => x.Has<Test>())
                .OrderBy(x => x.Name, StringComparer.Ordinal);

        public void Execute(TestClass testClass)
        {
            var instance = testClass.Construct();

            Execute<TestFixtureSetUp>(instance);
            foreach (var test in testClass.Tests)
            {
                if (test.HasParameters)
                {
                    foreach (var parameters in TestCaseSource.GetParameters(test.Method))
                        RunTestCaseLifecycle(instance, test, parameters);
                }
                else
                {
                    RunTestCaseLifecycle(instance, test);
                }
            };
            Execute<TestFixtureTearDown>(instance);

            instance.Dispose();
        }

        static void RunTestCaseLifecycle(object instance, TestMethod test, params object[] parameters)
        {
            Execute<SetUp>(instance);
            test.Run(parameters, instance, HandleExpectedExceptions);
            Execute<TearDown>(instance);
        }

        static void Execute<TAttribute>(object instance) where TAttribute : Attribute
        {
            var query = instance
                .GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(x => x.Has<TAttribute>());

            foreach (var q in query)
                q.Execute(instance);
        }

        static void HandleExpectedExceptions(Case @case)
        {
            var attribute = @case.Method.GetCustomAttributes<ExpectedExceptionAttribute>(false).SingleOrDefault();

            if (attribute == null)
                return;

            var exception = @case.Exception;

            try
            {
                if (exception == null)
                    throw new Exception("Expected exception of type " + attribute.ExpectedException + ".");

                if (!attribute.ExpectedException.IsAssignableFrom(exception.GetType()))
                {
                    throw new Exception(
                        "Expected exception of type " + attribute.ExpectedException + " but an exception of type " +
                        exception.GetType() + " was thrown.", exception);
                }

                if (attribute.ExpectedMessage != null && exception.Message != attribute.ExpectedMessage)
                {
                    throw new Exception(
                        "Expected exception message '" + attribute.ExpectedMessage + "'" + " but was '" + exception.Message + "'.",
                        exception);
                }

                @case.Pass();
            }
            catch (Exception failureException)
            {
                @case.Fail(failureException);
            }
        }
    }

    class TestCaseSource
    {
        public static IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            var testInvocations = new List<object[]>();

            var testCaseSourceAttributes = method.GetCustomAttributes<TestCaseSourceAttribute>(true).ToList();

            foreach (var attribute in testCaseSourceAttributes)
            {
                var sourceType = attribute.SourceType ?? method.DeclaringType;

                if (sourceType == null)
                    throw new Exception("Could not find source type for method " + method.Name);

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
}
