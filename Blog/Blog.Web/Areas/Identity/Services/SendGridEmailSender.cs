namespace Blog.Web.Areas.Identity.Services
{
    using System.Threading.Tasks;
    using Blog.Common.Infrastructure.Validation;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions options;

        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            this.options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(email, nameof(email));
            Validator.ThrowIfNullEmptyOrWhiteSpace(subject, nameof(subject));
            Validator.ThrowIfNullEmptyOrWhiteSpace(htmlMessage, nameof(htmlMessage));

            var client = new SendGridClient(this.options.SendGridApiKey);
            var from = new EmailAddress("admin@example.com", "Blogger Admin");
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
            var response = await client.SendEmailAsync(msg);
            var body = await response.Body.ReadAsStringAsync();
            var statusCode = response.StatusCode;
        }
    }
}
