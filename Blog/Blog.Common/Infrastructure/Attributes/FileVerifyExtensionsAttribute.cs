namespace Blog.Common.Infrastructure.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileVerifyExtensionsAttribute : ValidationAttribute
    {
        public FileVerifyExtensionsAttribute(string fileExtensions)
        {
            this.AllowedExtensions = fileExtensions
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private List<string> AllowedExtensions { get; }

        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;

            if (file != null)
            {
                var fileName = file.FileName;

                if (!this.AllowedExtensions.Any(y => fileName.EndsWith(y)))
                {
                    return false;
                }

                if (file.Length == 0 || file.Length > 716800)
                {
                    this.ErrorMessage = Messages.AllowedFileSize;
                    return false;
                }
            }

            return true;
        }
    }
}