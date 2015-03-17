using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Attributes;
using TransactRules.Core.Entities;

namespace TransactRules.Dox.Configuration
{
    public class DocumentTemplate:Entity
    {
        public virtual string TemplateName { get; set; }

        [Association(AssociationType.Contains, "", "DocumentTemplate")]
        public virtual IList<DocumentTemplateProperty> Properties { get; set; }

        public virtual int DocumentContentId { get; set; }
    }
}
