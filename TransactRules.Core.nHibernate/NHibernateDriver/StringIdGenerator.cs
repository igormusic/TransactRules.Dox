using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Engine;

namespace TransactRules.Core.NHibernateDriver
{
    public class StringIdGenerator : NHibernate.Id.IIdentifierGenerator
    {
        public object Generate(ISessionImplementor session, object obj)
        {
            var generator = new NHibernate.Id.GuidCombGenerator();

            var id = generator.Generate(session, obj);
            
            return id.ToString();
        }

    }
}
