namespace Blog.Common.Infrastructure.Validation
{
    using System;

    public class Validator
    {
        public static void ThrowIfNull(object obj, string message = "")
        {
            if (obj == null)
            {
                if (message == string.Empty)
                {
                    message = string.Format(ValidationConstants.ValueNullMsg, nameof(obj));
                }

                throw new ArgumentException(message);
            }
        }

        public static void ThrowIfNullEmptyOrWhiteSpace(string param, string paramName, string message = "")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (message == string.Empty)
                {
                    message = string.Format(ValidationConstants.ValueNullEmptyWhiteSpaceMsg, paramName);
                }

                throw new ArgumentException(message);
            }
        }

        public static void ThrowIfZeroOrNegative(int param, string paramName, string message = "")
        {
            if (param <= 0)
            {
                if (message == string.Empty)
                {
                    message = string.Format(ValidationConstants.ValueZeroOrNegativeMsg, paramName);
                }

                throw new ArgumentException(message);
            }
        } 
    }
}