using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Mailing.Domain;

namespace Coolector.Services.Mailing.Repositories
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