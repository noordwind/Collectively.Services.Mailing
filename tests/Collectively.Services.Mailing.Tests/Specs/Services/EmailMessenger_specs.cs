using Collectively.Services.Mailing.Domain;
using Collectively.Services.Mailing.Repositories;
using Collectively.Services.Mailing.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Collectively.Services.Mailing.Tests.Specs.Services
{
    public abstract class EmailMessenger_specs
    {
        protected static IEmailMessenger EmailMessenger;
        protected static Mock<ISendGridClient> SendGridClientMock;
        protected static Mock<IEmailTemplateRepository> EmailTemplateRepositoryMock;
        protected static SendGridSettings SendGridSettings;

        protected static void Initialize()
        {
            SendGridClientMock = new Mock<ISendGridClient>();
            EmailTemplateRepositoryMock = new Mock<IEmailTemplateRepository>();
            SendGridSettings = new SendGridSettings();
            EmailMessenger = new SendGridEmailMessenger(SendGridClientMock.Object,
                EmailTemplateRepositoryMock.Object, SendGridSettings);
        }
    }

    [Subject("EmailMessenger_ HandleAsync")]
    public class when_invoking_send_reset_password_async : EmailMessenger_specs
    {
        static string TemplateCodename;
        static EmailTemplate EmailTemplate;
        static string Email;
        static string Endpoint;
        static string Token;
        static string Culture;

        Establish context = () =>
        {
            Initialize();
            Email = "test@collectively.com";
            Endpoint = "set-new-password";
            Token = "xyz";
            Culture = "en-gb";
            TemplateCodename = EmailTemplates.ResetPassword;
            EmailTemplate = new EmailTemplate("Reset password", TemplateCodename, "id",
                Culture, "Reset password");
            EmailTemplateRepositoryMock.Setup(x => x.GetByCodenameAsync(TemplateCodename, Culture))
                .ReturnsAsync(EmailTemplate);
        };

        Because of = () => EmailMessenger.SendResetPasswordAsync(Email, Endpoint,
            Token, Culture).Await();

        It should_invoke_get_by_codename_async_on_email_template_repository = () =>
        {
            EmailTemplateRepositoryMock.Verify(x => x.GetByCodenameAsync(TemplateCodename, Culture), Times.Once);
        };

        It should_invoke_send_message_async_on_sendgrid_client = () =>
        {
            SendGridClientMock.Verify(x => x.SendMessageAsync(Moq.It.IsAny<SendGridEmailMessage>()), Times.Once);
        };
    }
}