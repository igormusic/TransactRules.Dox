using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Id;
using NHibernate.Dialect;
using NHibernate.Type;

namespace TransactRules.nHibernate.NHibernateDriver
{
    public class NHibIdGenerator : TableHiLoGenerator
    {
        private static readonly HashSet<string> typeNames = new HashSet<string>();

        public override void Configure(IType type, IDictionary<string, string> parms, Dialect dialect)
        {
            string typeName = parms["entity_name"];
            typeNames.Add(typeName);

            var newParms = new Dictionary<string, string>
            {
                {"table", "NextKey"},
                {"column", "NextHiValue"},
                {"max_lo", "10"},
                {"where", string.Format("TypeName='{0}'", typeName)},
            };

            base.Configure(type, newParms, dialect);
        }

        public override string[] SqlCreateStrings(Dialect dialect)
        {
            string[] commands = base.SqlCreateStrings(dialect);
            string create = commands[0];
            string insert = commands[1];

            var newCommands = new List<string>();

            newCommands.Add(
                string.Format(@"
CREATE TABLE [NextKey]
(
  [TypeName] {0},
  [NextHiValue] {1},
  primary key clustered ([TypeName])
)"
                    , dialect.GetTypeName(new global::NHibernate.SqlTypes.SqlType(System.Data.DbType.String, 255))
                    , dialect.GetTypeName(new global::NHibernate.SqlTypes.SqlType(System.Data.DbType.Int64))
                    ));

            foreach (string entityName in typeNames.OrderBy(x => x))
            {
                newCommands.Add(insert.Replace("( 1 )", string.Format("('{0}', 1)", entityName)));
            }

            return newCommands.ToArray();
        }
    }
}
