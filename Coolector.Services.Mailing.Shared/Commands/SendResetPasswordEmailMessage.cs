namespace Coolector.Services.Mailing.Shared.Commands
{
    public class SendResetPasswordEmailMessage : SendEmailMessageBase
    {
        public string Token { get; set; }
        public string Endpoint { get; set; }
    }
}