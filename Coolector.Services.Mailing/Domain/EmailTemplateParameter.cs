using System;
using System.Collections.Generic;
using System.Linq;

namespace Coolector.Services.Mailing.Domain
{
    public class EmailTemplateParameter
    {
        public string ReplacementTag { get; }
        public IEnumerable<string> Values { get; }

        protected EmailTemplateParameter(string replacementTag, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(replacementTag))
                throw new ArgumentException("Replacement tag can not be empty", nameof(replacementTag));
            if (values?.Any() == false)
                throw new ArgumentException("Replacement tag values can not be empty", nameof(replacementTag));

            ReplacementTag = replacementTag;
            Values = values;
        }

        public static EmailTemplateParameter Create(string replacementTag, params string[] values)
            => new EmailTemplateParameter(replacementTag, values);
    }
}