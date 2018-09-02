namespace Blog.Common.Author.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PostSearchBindingModel
    {
        public PostSearchBindingModel()
        {
            this.Categories = new List<SelectListItem>();
            this.Authors = new List<SelectListItem>();
        }

        [Display(Name = "Post Id")]
        public int? PostId { get; set; }

        public string Title { get; set; }

        [Display(Name = nameof(Category))]
        public int? CategoryId { get; set; }

        [Display(Name = Constants.Author)]
        public string AuthorId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = Constants.FromDateFieldName)]
        public DateTime? FromCreationDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = Constants.ToDateFieldName)]
        public DateTime? ToCreationDate { get; set; }

        [BindNever]
        [Display(Name = nameof(Category))]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [BindNever]
        [Display(Name = Constants.Author)]
        public IEnumerable<SelectListItem> Authors { get; set; }
    }
}
