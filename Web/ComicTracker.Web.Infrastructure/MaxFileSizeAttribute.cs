namespace ComicTracker.Web.Infrastructure
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static ComicTracker.Common.GlobalConstants;

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private const int KilobytesInAMegabyte = 1024;

        private readonly int maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        public string GetErrorMessage() =>
            $"Maximum allowed file size is {(this.maxFileSize / BytesInAKilobyte) / KilobytesInAMegabyte}MB.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > this.maxFileSize)
                {
                    return new ValidationResult(this.GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
    }
}
