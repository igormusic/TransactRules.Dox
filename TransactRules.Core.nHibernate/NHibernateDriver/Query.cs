using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using TransactRules.Core.Entities;

namespace TransactRules.Core.NHibernateDriver
{
    public class Query<T> : IQuery<T>
    {
        private ISession _session;

        public Query(ISession session)
        {
            _session = session;
        }

        public IQueryable<T> Items
        {
            get
            {
                return _session.Query<T>();
            }

        }

    }
}
