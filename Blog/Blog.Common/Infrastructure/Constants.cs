namespace Blog.Common.Infrastructure
{
    public class Constants
    {
        public const string Administrator = "Administrator";
        public const string Author = "Author";
        public const string DefaultAdminPassword = "@admin123";
        public const string DefaultLecturerPassword = "@author123";
        public const string StaticFilesFolder = "Static";
        public const string HashTag = "#";
        public const string PostsImgFolder = "Posts";
        public const string UsersImgFolder = "Users";
        public const string Jpg = ".jpg";
        public const string DefaultAvatarPath = "/users/avatar.jpg";
        public const string FileExtensions = "jpg,jpeg,png,gif";
        public const string TempDataKey = "__Message";
        public const string Business = "Business";
        public const string Technology = "Technology";
        public const string Lifestyle = "LifeStyle";
        public const string FromDateFieldName = "From Date of publish";
        public const string ToDateFieldName = "To Date of publish";

        public const int PasswordMinimumLength = 6;
        public const int PasswordMaxLength = 100;
        public const int HeadlineMinimumLength = 2;
        public const int HeadlineMaxLength = 150;
        public const int CategoryNameMaxLength = 25;
        public const int CategoryOrderMinimumLength = 1;
        public const int CategoryOrderMaxLength = 100;
    }
}
