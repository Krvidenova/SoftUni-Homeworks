namespace Blog.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Attributes;
    using Blog.Models;
    using Blog.Services.User.Interfaces;   
    using Blog.Web.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUserProfileService userService;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IUserProfileService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.userService = userService;
        }

        public string Username { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [FileVerifyExtensions(Constants.FileExtensions, ErrorMessage =
             "You can upload a file with the following file extensions: .jpg, .jpeg, .png, .gif")]
            [Display(Name = "Avatar")]
            public IFormFile AvatarFile { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [StringLength(100, MinimumLength = 2)]
            [Display(Name = "Name")]
            public string FullName { get; set; }

            [Url]
            [Display(Name = "Facebook Profile URL")]
            public string FacebookProfileUrl { get; set; }

            [Url]
            [Display(Name = "Twitter Profile URL")]
            public string TwitterProfileUrl { get; set; }

            [Url]
            [Display(Name = "Instagram Profile URL")]
            public string InstagramProfileUrl { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.AvatarUrl = user.AvatarUrl ?? Constants.DefaultAvatarPath;
            this.Username = await this.userManager.GetUserNameAsync(user);

            var email = await this.userManager.GetEmailAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var name = user.FullName;
            var facebookProfileUrl = user.FacebookProfileUrl;
            var twitterProfileUrl = user.TwitterProfileUrl;
            var instagramProfileUrl = user.InstagramProfileUrl;

            this.Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                FullName = name,
                FacebookProfileUrl = facebookProfileUrl,
                TwitterProfileUrl = twitterProfileUrl,
                InstagramProfileUrl = instagramProfileUrl
            };

            this.IsEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var userName = await this.userManager.GetUserNameAsync(user);
            this.Username = userName;
            this.AvatarUrl = user.AvatarUrl ?? Constants.DefaultAvatarPath;
            
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            this.Input = TSelfExtensions.TrimStringProperties(this.Input);
            var email = await this.userManager.GetEmailAsync(user);
            var userId = await this.userManager.GetUserIdAsync(user);
            if (this.Input.Email != email)
            {
                var setEmailResult = await this.userManager.SetEmailAsync(user, this.Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(email), nameof(this.User), userId));
                }
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(this.Input.PhoneNumber), nameof(this.User), userId));
                }
            }

            if (this.Input.AvatarFile != null)
            {
                bool setAvatarSucceeded = await this.userService
                    .UploadFileInFileSystemAsync(this.Input.AvatarFile, userId);
                if (!setAvatarSucceeded)
                {
                    throw new InvalidOperationException(string.Format(Messages.UnexpectedError, "Avatar", nameof(this.User), userId));
                }
            }

            if (this.User.IsInRole(Constants.Administrator) || this.User.IsInRole(Constants.Author))
            {
                var name = user.FullName;
                if (this.Input.FullName != null && this.Input.FullName != name)
                {
                    bool setNameSucceeded = await this.userService.SetNameAsync(userId, this.Input.FullName);
                    if (!setNameSucceeded)
                    {
                        throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(name), nameof(this.User), userId));
                    }
                }

                var facebookProfileUrl = user.FacebookProfileUrl;
                if (this.Input.FacebookProfileUrl != null && this.Input.FacebookProfileUrl != facebookProfileUrl)
                {
                    bool setFacebookProfileUrlSucceeded = await this.userService
                                                              .SetFacebookProfileUrlAsync(userId, this.Input.FacebookProfileUrl);
                    if (!setFacebookProfileUrlSucceeded)
                    {
                        throw new InvalidOperationException(string.Format(Messages.UnexpectedError, "Facebook profile Url", nameof(this.User), userId));
                    }
                }

                var twitterProfileUrl = user.TwitterProfileUrl;
                if (this.Input.TwitterProfileUrl != null && this.Input.TwitterProfileUrl != twitterProfileUrl)
                {
                    bool setTwitterProfileUrlSucceeded = await this.userService
                                                             .SetTwitterProfileUrlAsync(userId, this.Input.TwitterProfileUrl);
                    if (!setTwitterProfileUrlSucceeded)
                    {
                        throw new InvalidOperationException(string.Format(Messages.UnexpectedError, "Twitter profile Url", nameof(this.User), userId));
                    }
                }

                var instagramProfileUrl = user.InstagramProfileUrl;
                if (this.Input.InstagramProfileUrl != null && this.Input.InstagramProfileUrl != instagramProfileUrl)
                {
                    bool setInstagramProfileUrlSucceeded = await this.userService
                                                               .SetInstagramProfileUrlAsync(userId, this.Input.InstagramProfileUrl);
                    if (!setInstagramProfileUrlSucceeded)
                    {
                        throw new InvalidOperationException(string.Format(Messages.UnexpectedError, "Instagram profile Url", nameof(this.User), userId));
                    }
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }


            var userId = await this.userManager.GetUserIdAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: this.Request.Scheme);
            await this.emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your Blogger account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.StatusMessage = "Verification email sent. Please check your email.";
            return this.RedirectToPage();
        }
    }
}
