using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactRules.Core.Entities
{
    public interface IQuery
    {

    }

    public interface IQuery<T> : IQuery
    {
        IQueryable<T> Items { get; }
    }
}
