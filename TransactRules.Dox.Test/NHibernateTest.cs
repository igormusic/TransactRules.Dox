using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactRules.Core.NHibernateDriver;

namespace TransactRules.Dox.Test
{
    [TestClass]
    public class NHibernateTest
    {
        [TestMethod]
        public void CreateDatabase()
        {
            var mapper = new NHibernateReflectionMapper();

            var sql = mapper.ExportSchema();
        }
    }
}
