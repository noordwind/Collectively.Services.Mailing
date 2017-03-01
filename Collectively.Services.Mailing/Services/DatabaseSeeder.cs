using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Services.Mailing.Domain;
using Collectively.Services.Mailing.Repositories;

namespace Collectively.Services.Mailing.Services
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public DatabaseSeeder(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public async Task SeedAsync()
        {
            var templates = await _emailTemplateRepository.GetAllAsync();
            if (templates.HasValue && templates.Value.Any())
                return;

            var seedTemplates = new List<EmailTemplate>();
            seedTemplates.AddRange(GetResetPasswordTemplates());
            var tasks = seedTemplates.Select(x => _emailTemplateRepository.AddAsync(x));
            await Task.WhenAll(tasks);
        }

        private IEnumerable<EmailTemplate> GetResetPasswordTemplates()
        {
            yield return new EmailTemplate("Reset password", "reset_password",
                "4febd104-85b1-4b57-a07f-85805b4e4241", "en-gb", "Reset password");

            yield return new EmailTemplate("Resetowanie hasła", "reset_password",
                "f2d8bbfe-ed9b-4cd1-ba07-7f4e1541cb1a", "pl-pl", "Resetowanie hasła");
        }
    }
}