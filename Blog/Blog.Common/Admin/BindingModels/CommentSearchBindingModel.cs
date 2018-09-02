namespace Blog.Common.Admin.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Blog.Common.Infrastructure;

    public class CommentSearchBindingModel
    {
        [Required(ErrorMessage = Messages.FromDate)]
        [DataType(DataType.Date)]
        [Display(Name = Constants.FromDateFieldName)]
        public DateTime FromCreationDate { get; set; } = DateTime.Now.Date;

        [Required(ErrorMessage = Messages.ToDate)]
        [DataType(DataType.Date)]
        [Display(Name = Constants.ToDateFieldName)]
        public DateTime ToCreationDate { get; set; } = DateTime.Now.Date;
    }
}
