using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TransactRules.Dox.DataContracts.Configuration
{
    [DataContract]
    public class DocumentTemplate
    {
        [DataMember]
        public string TemplateName { get; set; }
    }
}
