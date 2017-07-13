using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Mailing.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Mailing.Repositories
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IMongoDatabase _database;

        public EmailTemplateRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<EmailTemplate>> GetByCodenameAsync(string codename, string culture)
            => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Codename == codename && x.Culture == culture);

        public async Task<Maybe<EmailTemplate>> GetByTemplateIdAsync(string templateId)
            => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.TemplateId == templateId);

        public async Task<Maybe<IEnumerable<EmailTemplate>>> GetAllAsync()
            => await Collection.AsQueryable().ToListAsync();

        public async Task AddAsync(EmailTemplate template)
            => await Collection.InsertOneAsync(template);

        public async Task UpdateAsync(EmailTemplate template)
            => await Collection.ReplaceOneAsync(x => x.Id == template.Id, template);

        private IMongoCollection<EmailTemplate> Collection
            => _database.GetCollection<EmailTemplate>();
    }
}