namespace Blog.Common.Infrastructure
{
    public class Messages
    {
        public const string AllowedFileSize = "The maximum allowed file size is 700kb";
        public const string CensoredComment = "The comment has been censored!";
        public const string UnexpectedError = "Unexpected error occurred setting {0} for {1} with ID: {2}.";
        public const string EntityDoesNotExist = "{0} does not exist.";
        public const string EntityCreateSuccess = "{0} with ID: {1} created successfully.";
        public const string EntityEditSuccess = "{0} with ID: {1} edited successfully.";
        public const string EntityDeleteSuccess = "{0} with ID: {1} deleted successfully.";
        public const string EntityApproveSuccess = "{0} with ID: {1} approved successfully.";
        public const string EntityModerateSuccess = "{0} with ID: {1} moderated successfully.";
        public const string NotAllowedMsg = "You are not allowed to perform this action.";
        public const string FillFormMsg = "Please fill in the form if you want to leave a comment.";
        public const string NotAllowedToComment = "You are not allowed to comment.";
        public const string CommentSuccess = "You created a comment successfully.";
        public const string FileVerifyError =
            "You can upload a file with the following file extensions: .jpg, .jpeg, .png, .gif";

        public const string ContentQuote =
            "Content: Please surround each quote together with the author of the quote as follows:  "
            + "STARTQUOTE  text quote ...  author ENDQUOTE.  "
            + "This is important for the proper visualization of the quoted text!";

        public const string TagsInput = "Tags: Please enter tags separated by comma \",\" Do Not add the symbol \"#\"!";
        public const string ContentRequired = "Content is required";
        public const string CreationDateInput = "Please fill in the correct date and time of publish.";
        public const string UpdateDateInput = "Please fill in the correct date and time of the update.";
        public const string FromDate = "The field \"From Date of publish\" is required.";
        public const string ToDate = "The field \"To Date of publish\" is required.";
        public const string PasswordError = "The {0} must be at least {2} and at max {1} characters long.";
        public const string ConfirmPasswordError = "The new password and confirmation password do not match.";
    }
}