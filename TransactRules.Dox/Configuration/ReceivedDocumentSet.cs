using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class ReceivedDocumentSet : Entity
    {
        public virtual ProcessType ProcessType { get; set; }
        public virtual string Name { get; set; }

        public virtual int RequiredNumberOfDocuments { get; set; }

        public virtual IList<ReceivedDocumentTypeReference> ReceivedDocumentTypes { get; set; }
    }
}
