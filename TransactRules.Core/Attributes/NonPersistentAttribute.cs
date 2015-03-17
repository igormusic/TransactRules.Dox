using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactRules.Core.Attributes
{
    [AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property)]
    public class NonPersistentAttribute : Attribute
    {
    }
}
