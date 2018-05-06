namespace Nested.Tests
{
    using Fixie;

    public class CustomConvention : Convention
    {
        public CustomConvention()
        {
            Classes
                .Where(x => x.Name.EndsWith("Tests"));
        }
    }
}