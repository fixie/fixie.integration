namespace Categories.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class TestingConvention : Discovery
    {
        public TestingConvention(string[] include)
        {
            var desiredCategories = include;
            var shouldRunAll = !desiredCategories.Any();

            Methods
                .Where(x => shouldRunAll || MethodHasAnyDesiredCategory(x, desiredCategories));

            if (!shouldRunAll)
            {
                Console.WriteLine("Categories: " + string.Join(", ", desiredCategories));
                Console.WriteLine();
            }
        }

        static bool MethodHasAnyDesiredCategory(MethodInfo method, string[] desiredCategories)
        {
            return Categories(method).Any(testCategory => desiredCategories.Contains(testCategory.Name));
        }

        static CategoryAttribute[] Categories(MethodInfo method)
        {
            return method.GetCustomAttributes<CategoryAttribute>(true).ToArray();
        }
    }
}