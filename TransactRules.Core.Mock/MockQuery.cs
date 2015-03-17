using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;

namespace TransactRules.Core.Mock
{
    public class MockQuery<T> : IQuery<T>
    {
        private IQueryable<T> _queryable;

        public MockQuery(IEnumerable<T> enumerable)
        {
            _queryable = (IQueryable<T>) enumerable.AsQueryable<T>();
        }

        public IQueryable<T> Items
        {
            get
            {
                return _queryable;
            }

        }

    }
}
