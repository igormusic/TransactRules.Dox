using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Attributes;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class ProcessType : Entity
    {
        public ProcessType()
        {
            ReceivedDocumentSets = new List<ReceivedDocumentSet>();
            GeneratedDocumentSets = new List<GeneratedDocumentSet>();
        }

        public string Name { get; set; }

        [Association(AssociationType.Contains, "", "ProcessType")]
        public virtual IList<ReceivedDocumentSet> ReceivedDocumentSets { get; set; }

        [Association(AssociationType.Contains, "", "ProcessType")]
        public virtual IList<GeneratedDocumentSet> GeneratedDocumentSets { get; set; }


    }
}
