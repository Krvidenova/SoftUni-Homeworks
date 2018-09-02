namespace Blog.Common.Admin.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Blog.Common.Infrastructure;

    public class UserChangePasswordBindingModel
    {
        [Required]
        public string Id { get; set; }

        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(Constants.PasswordMaxLength, ErrorMessage = Messages.PasswordError, MinimumLength = Constants.PasswordMinimumLength)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = Messages.ConfirmPasswordError)]
        public string ConfirmPassword { get; set; }
    }
}
