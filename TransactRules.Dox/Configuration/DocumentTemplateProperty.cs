using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox.Configuration
{
    public class DocumentTemplateProperty:Entity
    {
        public virtual string PropertyName { get; set; }
    }
}
