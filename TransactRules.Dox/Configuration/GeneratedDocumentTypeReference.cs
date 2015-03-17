using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class GeneratedDocumentTypeReference : Entity
    {
        public virtual GeneratedDocumentSet GeneratedDocumentSet { get; set; }

        public virtual GeneratedDocumentType GeneratedDocumentType { get; set; }

    }
}
