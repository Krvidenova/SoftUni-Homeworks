namespace Blog.Common.Admin.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Blog.Common.Infrastructure;

    public class CategoryEditBindingModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.CategoryNameMaxLength, MinimumLength = Constants.HeadlineMinimumLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.CategoryOrderMinimumLength, Constants.CategoryOrderMaxLength)]
        public int Order { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.HeadlineMaxLength, MinimumLength = Constants.HeadlineMinimumLength)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}