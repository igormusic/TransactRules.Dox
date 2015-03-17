using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Xml.Linq;
using TransactRules.Core;
using TransactRules.Core.Utility;
using TransactRules.Core.Entities;

namespace TransactRules.Core.NHibernateDriver
{
    // Helper class to configure and create/provide session
    internal static class SessionManager
    {
        private static ISessionFactory _sessionFactory;
        private static Object _lock = new Object();

        public static ISessionFactory SessionFactory
        {
            get
            {
                lock (_lock) 
                {
                    if (_sessionFactory == null)
                    {

                        var types = (from type in TypeEnumerator.Types
                                     where typeof(Entity).IsAssignableFrom(type)
                                       && type != typeof(Entity)
                                     select type);

                        var mapper = new NHibernateReflectionMapper();

                        mapper.Configure(types, "");

                        _sessionFactory = mapper.Configuration.BuildSessionFactory();
                    }                
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
