using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class ReceivedDocumentTypeReference : Entity
    {
        public virtual ReceivedDocumentSet ReceivedDocumentSet { get; set; }

        public virtual ReceivedDocumentType ReceivedDocumentType { get; set; }
    }
}
