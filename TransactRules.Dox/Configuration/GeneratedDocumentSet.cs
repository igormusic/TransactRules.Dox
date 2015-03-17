using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class GeneratedDocumentSet : Entity
    {
        public virtual ProcessType ProcessType { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<GeneratedDocumentTypeReference> GeneratedDocumentTypes { get; set; }
    }
}
