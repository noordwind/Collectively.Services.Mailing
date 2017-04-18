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
            yield return new EmailTemplate("Reset password", EmailTemplates.ResetPassword,
                "4febd104-85b1-4b57-a07f-85805b4e4241", "en-gb", "Reset password");

            yield return new EmailTemplate("Resetowanie hasła", EmailTemplates.ResetPassword,
                "f2d8bbfe-ed9b-4cd1-ba07-7f4e1541cb1a", "pl-pl", "Resetowanie hasła");
        }

        private IEnumerable<EmailTemplate> GetRemarkCreatedTemplates()
        {
            yield return new EmailTemplate("Remark created", EmailTemplates.RemarkCreated,
                "fdd82538-cb92-47ad-9258-a0bcb126505e", "en-gb",
                "Collectively - Remark created");
            yield return new EmailTemplate("Remark created", EmailTemplates.RemarkCreated,
                "7b1d8a8c-4d03-406f-ac56-a3f278a43b0d", "pl-pl",
                "Collectively - Zgłoszenie dodane");
        }

        private IEnumerable<EmailTemplate> GetRemarkStateChangedTemplates()
        {
            yield return new EmailTemplate("Remark state changed", EmailTemplates.RemarkStateChanged,
                "783a186e-5caf-458d-98db-b22f8a04583b", "en-gb", 
                "Collectively - Remark state changed");
            yield return new EmailTemplate("Remark state changed", EmailTemplates.RemarkStateChanged,
                "5f558a60-bcfc-498a-88d2-decb41b6265b", "pl-pl",
                "Collectively - Nowy status zgłoszenia");
        }

        private IEnumerable<EmailTemplate> GetPhotosAddedToRemarkTemplates()
        {
            yield return new EmailTemplate("Photos added to remark", EmailTemplates.PhotosAddedToRemark,
                "f85beaed-6bf1-4b2d-b088-ae2617ed6cce", "en-gb",
                "Collectively - New photos added to remark");
            yield return new EmailTemplate("Photos added to remark", EmailTemplates.PhotosAddedToRemark,
                "c0ebffbc-7bbc-4b1d-9238-733541cefa71", "pl-pl",
                "Collectively - Dodano nowe zdjęcie do zgłoszenia");
        }

        private IEnumerable<EmailTemplate> GetCommentAddedToRemarkTemplates()
        {
            yield return new EmailTemplate("Comment added to remark", EmailTemplates.CommentAddedToRemark,
                "087a9ca3-dc1a-4b91-b985-496f82279690", "en-gb",
                "Collectively - New comments added to remark");
            yield return new EmailTemplate("Comment added to remark", EmailTemplates.CommentAddedToRemark,
                "e84b81d2-b398-44bb-a41d-4526b4af7bfc", "pl-pl",
                "Collectively - Nowy komentarz do zgłoszenia");
        }
    }
}