using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Domain;
using Collectively.Common.Extensions;
using Collectively.Services.Mailing.Domain;
using Collectively.Services.Mailing.Repositories;


namespace Collectively.Services.Mailing.Services
{
    public class SendGridEmailMessenger : IEmailMessenger
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly SendGridSettings _sendGridSettings;

        public SendGridEmailMessenger(ISendGridClient sendGridClient,
            IEmailTemplateRepository emailTemplateRepository,
            SendGridSettings sendGridSettings)
        {
            _sendGridClient = sendGridClient;
            _emailTemplateRepository = emailTemplateRepository;
            _sendGridSettings = sendGridSettings;
        }

        public async Task SendResetPasswordAsync(string email, string endpoint, string token, string culture)
        {
            var template = await GetTemplateOrFallbackToDefaultOrFailAsync(EmailTemplates.ResetPassword, culture);
            var resetPasswordUrl = $"{endpoint}?email={email}&token={token}";
            var emailMessage = CreateMessage(email, template.Subject);
            ApplyTemplate(template.TemplateId, emailMessage,
                EmailTemplateParameter.Create("resetPasswordUrl", resetPasswordUrl));
            await _sendGridClient.SendMessageAsync(emailMessage);
        }

        private async Task<EmailTemplate> GetTemplateOrFallbackToDefaultOrFailAsync(string codename, string culture)
        {
            var template = await _emailTemplateRepository.GetByCodenameAsync(codename, culture);
            if (template.HasValue)
                return template.Value;

            template = await _emailTemplateRepository.GetByCodenameAsync(codename, _sendGridSettings.DefaultCulture);
            if (template.HasValue)
                return template.Value;

            throw new ServiceException(OperationCodes.EmailTemplateNotFound,
                $"Email template: '{codename}' has not been found.");
        }

        private SendGridEmailMessage CreateMessage(string receiver,
            string subject, string message = null)
        {
            var emailMessage = new SendGridEmailMessage
            {
                From = new SendGridEmailMessage.Person
                {
                    Email = _sendGridSettings.NoReplyEmailAccount
                },
                Subject = subject,
                Personalizations = new List<SendGridEmailMessage.Personalization>()
            };
            emailMessage.Personalizations.Add(new SendGridEmailMessage.Personalization
            {
                To = new List<SendGridEmailMessage.Person>
                {
                    new SendGridEmailMessage.Person
                    {
                        Email = receiver
                    }
                },
                Substitutions = new Dictionary<string, string>()
            });
            if (message.NotEmpty())
            {
                emailMessage.Content = new List<SendGridEmailMessage.MessageContent>()
                {
                    new SendGridEmailMessage.MessageContent
                    {
                        Value = message
                    }
                };
            }

            return emailMessage;
        }

        private void ApplyTemplate(string templateId, SendGridEmailMessage emailMessage,
            params EmailTemplateParameter[] parameters)
        {
            emailMessage.Content = null;
            emailMessage.TemplateId = templateId;
            var personalization = emailMessage.Personalizations.First();
            foreach (var parameter in parameters)
            {
                var parameterValue = string.Format("{0}", parameter.Values.FirstOrDefault());
                personalization.Substitutions[$"-{parameter.ReplacementTag}-"] = parameterValue;
            }
        }
    }
}