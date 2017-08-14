namespace CustomConvention.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    class ParameterizedTestsConvention : Convention
    {
        public ParameterizedTestsConvention()
        {
            Classes
                .NameEndsWith("Tests");

            Methods
                .Where(method => method.IsVoid());

            ClassExecution
                .CreateInstancePerClass()
                .SortCases((caseA, caseB) => String.Compare(caseA.Name, caseB.Name, StringComparison.Ordinal));

            CaseExecution
                .Skip(new SkipDueToExplicitAttribute(IsTarget))
                .Skip<SkipDueToClassLevelSkipAttribute>()
                .Skip<SkipDueToMethodLevelSkipAttribute>();

            Parameters
                .Add<InputAttributeParameterSource>();
        }
    }

    class SkipDueToExplicitAttribute : SkipBehavior
    {
        private readonly Func<MethodInfo, bool> isTargetMethod;

        public SkipDueToExplicitAttribute(Func<MethodInfo, bool> isTargetMethod)
        {
            this.isTargetMethod = isTargetMethod;
        }

        public bool SkipCase(Case @case)
        {
            var method = @case.Method;

            var isMarkedExplicit = method.Has<ExplicitAttribute>();

            return isMarkedExplicit && !isTargetMethod(method);
        }

        public string GetSkipReason(Case @case)
            => "[Explicit] tests run only when they are individually selected for execution.";
    }

    class SkipDueToClassLevelSkipAttribute : SkipBehavior
    {
        public bool SkipCase(Case @case)
            => @case.Method.DeclaringType.HasOrInherits<SkipAttribute>();

        public string GetSkipReason(Case @case)
            => @case.Method.DeclaringType.GetCustomAttribute<SkipAttribute>().Reason;
    }

    class SkipDueToMethodLevelSkipAttribute : SkipBehavior
    {
        public bool SkipCase(Case @case)
            => @case.Method.HasOrInherits<SkipAttribute>();

        public string GetSkipReason(Case @case)
            => @case.Method.GetCustomAttribute<SkipAttribute>().Reason;
    }

    class InputAttributeParameterSource : ParameterSource
    {
        public IEnumerable<object[]> GetParameters(MethodInfo method)
            => method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);
    }
}