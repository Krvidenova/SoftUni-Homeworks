namespace Blog.Web.Extensions
{
    using System;

    public static class StringExtensions
    {       
        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
            {
                return input;
            }

            int indexNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}...", input.Substring(0, (indexNextSpace > 0) ? indexNextSpace : length).Trim());
        }
    }
}
