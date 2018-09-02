namespace Blog.Common.Author.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Infrastructure.Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PostCreateBindingModel
    {
        public PostCreateBindingModel()
        {
            this.Categories = new List<SelectListItem>();
            this.CreationDate = DateTime.Now;
        }      

        [FileVerifyExtensions(Constants.FileExtensions, ErrorMessage = Messages.FileVerifyError)]
        public IFormFile Image { get; set; }        

        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.HeadlineMaxLength, MinimumLength = Constants.HeadlineMinimumLength)]
        public string Headline { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Messages.ContentRequired)]
        [DataType(DataType.MultilineText)]
        [Display(Name = Messages.ContentQuote)]
        public string Content { get; set; }       

        [Required(ErrorMessage = Messages.CreationDateInput)]
        [Display(Name = "Date and Time of publish")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

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