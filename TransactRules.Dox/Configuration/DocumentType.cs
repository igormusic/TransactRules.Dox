using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox
{
    public class DocumentType:Entity
    {
        public virtual string Name { get; set; }
        [StringLength(1024)]
        public virtual string Description { get; set; }

        public virtual bool HasExpiryDate { get; set; }
    }
}
