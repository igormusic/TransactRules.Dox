using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Dox.Configuration
{
    public class DocumentContent:Entity
    {
        public virtual string MimeType { get; set; }
        public virtual byte[] Content { get; set; }
    }
}
