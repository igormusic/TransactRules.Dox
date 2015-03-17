using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class GeneratedDocumentType : DocumentType
    {
        public virtual bool RequireSignature { get; set; }

        public virtual bool IsValidForOtherProcesses { get; set; }

        public virtual int DocumentTemplate { get; set; }

    }
}
