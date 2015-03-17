using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = TransactRules.Dox.DataContracts.Configuration;
using domain = TransactRules.Dox.Configuration;

namespace TransactRules.Dox.Mapping
{
    public static class DocumentTemplateMapper
    {
        public static dto.DocumentTemplate ToDTO(this domain.DocumentTemplate documentTemplate)
        {
           return new dto.DocumentTemplate { 
               TemplateName = documentTemplate.TemplateName 
           };

        }

        public static IEnumerable<dto.DocumentTemplate> ToDTO(this IEnumerable<domain.DocumentTemplate> documentTemplates) {
            return (from documentTemplate in documentTemplates
                    select documentTemplate.ToDTO()).ToArray();
        
        }

        public static domain.DocumentTemplate ToDomain(this dto.DocumentTemplate documentTemplate)
        {
            return new domain.DocumentTemplate
            {
                TemplateName = documentTemplate.TemplateName
            };

        }

    }   
}
