namespace Blog.Web.Extensions
{
    using System.Linq;

    public static class TSelfExtensions
    {
        public static TSelf TrimStringProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                {
                    stringProperty.SetValue(input, currentValue.Trim(), null);
                }
            }

            return input;
        }
    }
}
