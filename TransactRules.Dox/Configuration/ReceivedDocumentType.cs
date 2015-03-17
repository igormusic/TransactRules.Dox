using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class ReceivedDocumentType:DocumentType
    {

        public virtual int ValidForMonths { get; set; }
    }
}
