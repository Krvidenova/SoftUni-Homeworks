namespace Blog.Common.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserDetailsViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        [Display(Name = "Avatar")]
        public string AvatarUrl { get; set; }

        [Display(Name = "Facebook")]
        public string FacebookProfileUrl { get; set; }

        [Display(Name = "Twitter")]
        public string TwitterProfileUrl { get; set; }

        [Display(Name = "Instagram")]
        public string InstagramProfileUrl { get; set; }

        public bool IsBanned { get; set; }

        [Display(Name = "Count of censored comments")]
        public int CensoredCommentsCount { get; set; }

        [Display(Name = "Count of deleted comments")]
        public int DeletedCommentsCount { get; set; }

        [Display(Name = "Comments Count")]
        public int CommentsCount { get; set; }

        [Display(Name = "Replies Count")]
        public int RepliesCount { get; set; }

        [Display(Name = "Posts Count")]
        public int PostsCount { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
