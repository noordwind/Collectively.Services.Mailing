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
            seedTemplates.AddRange(GetRemarkCreatedTemplates());
            seedTemplates.AddRange(GetRemarkStateChangedTemplates());
            seedTemplates.AddRange(GetPhotosAddedToRemarkTemplates());
            seedTemplates.AddRange(GetCommentAddedToRemarkTemplates());
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

        private IEnumerable<EmailTemplate> GetRemarkCreatedTemplates()
        {
            yield return new EmailTemplate("Remark created", "remark_created",
                "fdd82538-cb92-47ad-9258-a0bcb126505e", "en-gb",
                "Collectively - Remark created");
        }

        private IEnumerable<EmailTemplate> GetRemarkStateChangedTemplates()
        {
            yield return new EmailTemplate("Remark state changed", "remark_sate_changed",
                "783a186e-5caf-458d-98db-b22f8a04583b", "en-gb", 
                "Collectively - Remark state changed");
        }

        private IEnumerable<EmailTemplate> GetPhotosAddedToRemarkTemplates()
        {
            yield return new EmailTemplate("Photos added to remark","photos_added_to_remark",
                "f85beaed-6bf1-4b2d-b088-ae2617ed6cce", "en-gb",
                "Collectively - New photos added to remark");
        }

        private IEnumerable<EmailTemplate> GetCommentAddedToRemarkTemplates()
        {
            yield return new EmailTemplate("Comment added to remark", "comment_added_to_remark",
                "087a9ca3-dc1a-4b91-b985-496f82279690", "en-gb",
                "Collectively - New comments added to remark");
        }
    }
}