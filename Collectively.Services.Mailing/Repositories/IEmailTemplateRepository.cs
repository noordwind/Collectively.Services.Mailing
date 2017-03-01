using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Mailing.Domain;

namespace Collectively.Services.Mailing.Repositories
{
    public interface IEmailTemplateRepository
    {
        Task<Maybe<EmailTemplate>> GetByCodenameAsync(string codename, string culture);
        Task<Maybe<EmailTemplate>> GetByTemplateIdAsync(string templateId);
        Task<Maybe<IEnumerable<EmailTemplate>>> GetAllAsync();
        Task AddAsync(EmailTemplate template);
        Task UpdateAsync(EmailTemplate template);
    }
}