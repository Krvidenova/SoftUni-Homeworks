namespace Blog.Common.Author.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Attributes;
    using Blog.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PostEditBindingModel
    {
        public PostEditBindingModel()
        {
            this.LastUpdateDate = DateTime.Now;
            this.Categories = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [FileVerifyExtensions(Constants.FileExtensions, ErrorMessage = Messages.FileVerifyError)]
        public IFormFile Image { get; set; }    
        
        public string ImageUrl { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.HeadlineMaxLength, MinimumLength = Constants.HeadlineMinimumLength)]
        public string Headline { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Messages.ContentRequired)]
        [DataType(DataType.MultilineText)]
        [Display(Name = Messages.ContentQuote)]
        public string Content { get; set; }

        public string CreationDate { get; set; }

        [Required(ErrorMessage = Messages.UpdateDateInput)]
        [Display(Name = "Date and Time of Update")]
        [DataType(DataType.DateTime)]
        public DateTime LastUpdateDate { get; set; }

        [Required]
        [Display(Name = nameof(Category))]
        public int CategoryId { get; set; }

        [Display(Name = Messages.TagsInput)]
        public string Tags { get; set; }

        [BindNever]
        [Display(Name = nameof(Category))]
        public IEnumerable<SelectListItem> Categories { get; set; } 
    }
}
