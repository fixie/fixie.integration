namespace Static.Tests
{
    using Fixie.Conventions;

    public class CustomConvention : DefaultConvention
    {
        public CustomConvention()
        {
            Classes
                .Where(x => x.Name.EndsWith("Tests"));
        }
    }
}