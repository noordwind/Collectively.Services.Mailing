using System;
using Collectively.Common.Domain;
using Collectively.Common.Extensions;

namespace Collectively.Services.Mailing.Domain
{
    public class EmailTemplate : IdentifiableEntity, ITimestampable
    {
        public string TemplateId { get; protected set; }
        public string Name { get; protected set; }
        public string Codename { get; protected set; }
        public string Subject { get; protected set; }
        public string Version { get; protected set; }
        public string Culture { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        public EmailTemplate(string name, string codename, string templateId,
            string culture, string subject, string version = null)
        {
            SetName(name);
            SetCodename(codename);
            SetTemplateId(templateId);
            SetCulture(culture);
            SetSubject(subject);
            SetVersion(version);
            CreatedAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (name.Empty())
            {
                throw new ArgumentException("Email template name can not be empty.", nameof(name));
            }

            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCodename(string codename)
        {
            if (codename.Empty())
            {
                throw new ArgumentException("Email template codename can not be empty.", nameof(codename));
            }

            Codename = codename.TrimToLower();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetTemplateId(string templateId)
        {
            if (templateId.Empty())
            {
                throw new ArgumentException("Email template id can not be empty.", nameof(templateId));
            }

            TemplateId = templateId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCulture(string culture)
        {
            if (culture.Empty())
            {
                throw new ArgumentException("Email template culture can not be empty.", nameof(culture));
            }

            Culture = culture.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetSubject(string subject)
        {
            if (subject.Empty())
            {
                throw new ArgumentException("Email template subject can not be empty.", nameof(subject));
            }

            Subject = subject;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetVersion(string version)
        {
            Version = version?.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}