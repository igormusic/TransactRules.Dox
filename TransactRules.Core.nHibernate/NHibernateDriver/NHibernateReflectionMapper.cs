using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.ComponentModel.DataAnnotations;
using TransactRules.Core.Entities;
using TransactRules.Core.Attributes;
using TransactRules.Core.Utility;

namespace TransactRules.Core.NHibernateDriver
{
    public class NHibernateReflectionMapper 
    {

        private class XDocumentBuilder
        {
            private XDocument document;
            private Stack<XElement> elementsStack = new Stack<XElement>();
            private XNamespace defaultNamespace;
            private XElement currentElement;

            public XDocumentBuilder(string @namespace)
            {
                document = new XDocument();

                defaultNamespace = @namespace;
            }

            public XDocument XDocument
            {
                get { return document; }
            }

            public XDocumentBuilder Element(string name)
            {
                XElement element;

                element = new XElement(defaultNamespace + name);

                if (currentElement == null)
                    document.Add(element);
                else
                    currentElement.Add(element);

                currentElement = element;

                elementsStack.Push(currentElement);

                return this;
            }

            public XDocumentBuilder Element(string name, string content)
            {
                XElement element;

                element = new XElement(defaultNamespace + name);

                if (currentElement == null)
                    document.Add(element);
                else
                    currentElement.Add(element);

                element.Value = content;

                currentElement = element;

                elementsStack.Push(currentElement);

                return this;
            }

            public XDocumentBuilder Attribute(string name, string value)
            {
                XAttribute attribute;

                attribute = new XAttribute(name, value);

                currentElement.Add(attribute);

                return this;
            }

            public XDocumentBuilder AttributeIf(string name, string value, bool condition)
            {
                if (!condition)
                    return this;

                XAttribute attribute;

                attribute = new XAttribute(name, value);

                currentElement.Add(attribute);

                return this;
            }

            public XDocumentBuilder EndElement()
            {
                if (elementsStack.Count == 0)
                {
                    currentElement = null;
                    return this;
                }

                elementsStack.Pop();

                if (elementsStack.Count == 0)
                {
                    currentElement = null;
                    return this;
                }

                currentElement = elementsStack.Peek();

                return this;
            }

        }

        private const int DefaultStringLength = 50;

        private NHibernate.Cfg.Configuration configuration;

        private string _tablePrefix;

        public void Configure(IEnumerable<Type> typesToMap, string tablePrefix)
        {
            _tablePrefix = tablePrefix;

            configuration = new NHibernate.Cfg.Configuration();

            configuration.Configure();
            
            var xml = BuildMappingXml(typesToMap);

            configuration.AddXml(xml);
            
        }

        private string BuildMappingXml(IEnumerable<Type> typesToMap)
        {
            var builder = new XDocumentBuilder("urn:nhibernate-mapping-2.2");

            builder.Element("hibernate-mapping")
                .Attribute("default-cascade", "save-update");


            foreach (var type in typesToMap)
            {
                MapType(type, builder);
            }

            builder
                .EndElement();


            var xml = builder.XDocument.ToString();
            return xml;
        }

        public NHibernate.Cfg.Configuration Configuration 
        { 
            get
            {
                return configuration;
            } 
        }

        private void MapType(Type type, XDocumentBuilder builder)
        {
            if (IsBaseType(type))
                MapEntityProperties(type, builder);
            else
                MapSubclass(type, builder);

            var properties = from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             where (!property.IsDefined(typeof(NonPersistentAttribute), false)) &&
                                   property.DeclaringType == type
                             select property;


            foreach (var property in properties)
                MapProperty(property, builder);

            builder.EndElement();
        }

        private static bool IsBaseType(Type type)
        {
            return type.BaseType == typeof(object) || type.BaseType == typeof(Entity);
        }

        private void MapSubclass(Type type, XDocumentBuilder builder)
        {
            builder
                .Element("subclass")
                .Attribute("name", FullTypeName(type))
                .Attribute("extends", FullTypeName(type.BaseType));
        }

        private void MapEntityProperties(Type type, XDocumentBuilder builder)
        {
            builder
                .Element("class")
                    .Attribute("name", FullTypeName(type))
                    .Attribute("lazy", "false")
                    .Attribute("table", string.Format("{0}{1}", _tablePrefix, GetDatabaseName(type.Name)))

                    //.Element("id")
                //    .Attribute("name", "Id")
                //    .Element("generator")
                //        .Attribute("class", "identity") // database identity
                //    .EndElement()
                //.EndElement()

                   //.Element("id")
                   //     .Attribute("name", "Id")
                   //     .Element("generator")
                   //         .Attribute("class", FullTypeName(typeof(NHibernate.Id.GuidCombGenerator))) // database identity
                   //     .EndElement()
                   // .EndElement()


                    //<id name="Id" type="Int64" column="cat_id">
                    //        <generator class="hilo">
                    //                <param name="table">hi_value</param>
                    //                <param name="column">next_value</param>
                    //                <param name="max_lo">100</param>
                    //        </generator>
                    //</id>

                    //.Element("id")
                    //    .Attribute("name", "Id")
                    //    .Attribute("unsaved-value","0")
                    //    .Element("generator")
                    //        .Attribute("class", "hilo") // database identity
                    //        .Element("param","hi_value")
                    //            .Attribute("name", "table")
                    //        .EndElement()
                    //        .Element("param","next_value")
                    //            .Attribute("name", "column")
                    //        .EndElement()
                    //        .Element("param","100")
                    //            .Attribute("name", "max_lo")
                    //        .EndElement()
                    //    .EndElement()
                    //.EndElement()

                    //<id name="Id">
                    //  <generator class="TransactRules.nHibernate.NHibernateDriver.NHibIdGenerator, TransactRules.nHibernate" />
                    //</id>

                   .Element("id")
                        .Attribute("name", "Id")
                        .Element("generator")
                            .Attribute("class", FullTypeName(typeof(TransactRules.nHibernate.NHibernateDriver.NHibIdGenerator))) // database identity
                        .EndElement()
                    .EndElement()                    

                    .Element("discriminator")
                    .EndElement()

                    .Element("version")
                        .Attribute("name", "Version")
                        .Attribute("unsaved-value", "null")
                    .EndElement()

                    .Element("property")
                        .Attribute("name", "CreatedBy")
                        .Element("column")
                            .Attribute("name", GetDatabaseName("CreatedBy"))
                        .EndElement()
                    .EndElement()

                    .Element("property")
                        .Attribute("name", "CreatedOn")
                        .Attribute("not-null", "true")
                        .Element("column")
                            .Attribute("name", GetDatabaseName("CreatedOn"))
                        .EndElement()
                    .EndElement()

                    .Element("property")
                        .Attribute("name", "LastUpdatedOn")
                        .Attribute("not-null", "false")
                        .Element("column")
                            .Attribute("name", GetDatabaseName("LastUpdatedOn"))
                        .EndElement()
                    .EndElement();
        }

        

        private void MapProperty(PropertyInfo property, XDocumentBuilder builder)
        {
            if (property.PropertyType == typeof(string))
            {
                MapString(property, builder);
            }
            else if (property.PropertyType.IsValueType == true)
            {
                MapValueType(property, builder);
            }
            else if (property.PropertyType== typeof(byte[]))
            {
                MapBinaryArray(property, builder);
            }
            else if (typeof(Entity).IsAssignableFrom(property.PropertyType))
            {
                MapOneAssociation(property, builder);
            }
            else if ((property.PropertyType.Name == "ISet`1" || property.PropertyType.Name == "IList`1")  &&
                     typeof(Entity).IsAssignableFrom(property.PropertyType.GetGenericArguments()[0]))
            {
                MapManyAssociation(property, builder);
            }
            else if (property.PropertyType == typeof(Object))
            {
                MapObjectOneAssociation(property, builder);
            }
            else
            {
                throw new ApplicationException(
                    String.Format(
                        "Persistent property {0} defined on type {1} has no defined automapping",
                        property.Name, property.DeclaringType.Name));
            }

        }

        private void MapOneAssociation(PropertyInfo property, XDocumentBuilder builder)
        {
            var associationAttribute =
                (AssociationAttribute)
                property.GetCustomAttributes(typeof(AssociationAttribute), true).FirstOrDefault();

            var isContained = false;

            if (associationAttribute != null && associationAttribute.Name == AssociationType.Contains)
                isContained = true;

            if (property.PropertyType == typeof(Entity))
                MapObjectOneAssociation(property, builder);
            else
                builder
                    .Element("many-to-one")
                    .Attribute("name", property.Name)
                    //.Attribute("lazy","proxy")
                    .AttributeIf("cascade", "all-delete-orphan", isContained)
                        .Element("column")
                            .Attribute("name", string.Format("{0}", GetDatabaseName(property.Name)))
                        .EndElement()
                    .EndElement();
        }

        private void MapObjectOneAssociation(PropertyInfo property, XDocumentBuilder builder)
        {
                builder
                    .Element("any")
                    .Attribute("name", property.Name)
                    .Attribute("id-type","int")
                        .Element("column")
                            .Attribute("name", "EntityType")
                            .Attribute("length", "255")
                        .EndElement()
                        .Element("column")
                            .Attribute("name", string.Format("{0}", GetDatabaseName(property.Name)))
                        .EndElement()
                    .EndElement();
        }

        private void MapManyAssociation(PropertyInfo property, XDocumentBuilder builder)
        {
            var elementType = property.PropertyType.GetGenericArguments()[0];

            var associationAttribute =
                (AssociationAttribute)
                property.GetCustomAttributes(typeof(AssociationAttribute), true).FirstOrDefault();

            if (associationAttribute == null)
            {
                builder
                  .Element("bag")
                      .Attribute("name", property.Name)
                      .Attribute("cascade", "all-delete-orphan") // default to  AssociationType.Contains
                      .Element("key")
                        .Attribute("column", string.Format("{0}", GetDatabaseName(property.DeclaringType.Name)))// default other column to this type
                      .EndElement()
                      .Element("one-to-many")
                        .Attribute("class", FullTypeName(elementType))
                      .EndElement()
                  .EndElement();

            }
            else
            {
                builder
                  .Element("bag")
                      .Attribute("name", property.Name)
                      .AttributeIf("cascade", "all-delete-orphan", associationAttribute.Name == AssociationType.Contains)
                      .Element("key")
                        .Attribute("column", string.Format("{0}", GetDatabaseName(associationAttribute.OtherKey)))
                      .EndElement()
                      .Element("one-to-many")
                        .Attribute("class", FullTypeName(elementType))
                      .EndElement()
                  .EndElement();
            }


        }

        private void MapValueType(PropertyInfo property, XDocumentBuilder builder)
        {
            builder
                .Element("property")
                .Attribute("name", property.Name)
                .AttributeIf("not-null", "true", (property.Name != "Nullable`1"))
                .AttributeIf("not-null", "false", (property.Name == "Nullable`1"))
                    .Element("column")
                        .Attribute("name", string.Format("{0}", GetDatabaseName( property.Name)))
                    .EndElement()
                .EndElement();

            //<column name="ParameterName" length="50" />
        }

        private void MapBinaryArray(PropertyInfo property, XDocumentBuilder builder)
        {

            var dialect = (from prop in Configuration.Properties
                           where prop.Key == "dialect"
                           select prop.Value).FirstOrDefault();

            var blobType = "BLOB";

            if (!String.IsNullOrEmpty(dialect) && ( dialect.StartsWith("NHibernate.Dialect.MsSql20" )))
                blobType = "image";


            builder
                .Element("property")
                    .Attribute("name", property.Name)
                    .Attribute("type", "BinaryBlob")
                        .Element("column")
                            .Attribute("name", string.Format("{0}", GetDatabaseName(property.Name)))
                            .Attribute("sql-type", blobType)
                        .EndElement()
                .EndElement();
        }

        private void MapString(PropertyInfo property, XDocumentBuilder builder)
        {
            var length = DefaultStringLength;

            var sizeAttribute =
                (StringLengthAttribute)
                property.GetCustomAttributes(typeof(StringLengthAttribute), true).FirstOrDefault();

            if (sizeAttribute != null)
                length = sizeAttribute.MaximumLength;

            if (length > 4000)
                builder
                    .Element("property")
                    .Attribute("name", property.Name)
                    .Attribute("type", "StringClob")
                    .Element("column")
                    .Attribute("name", GetDatabaseName(property.Name))
                    .Attribute("sql-type", "NVARCHAR(MAX)")
                    .EndElement()
                    .EndElement();
            else
                builder
                    .Element("property")
                    .Attribute("name", property.Name)
                    .Element("column")
                    .Attribute("name", GetDatabaseName(property.Name))
                    .Attribute("length", length.ToString())
                    .EndElement()
                    .EndElement();
        }

        private string GetDatabaseName(string name)
        {
            return string.Format("[{0}]",name);
        
        }
        private string ExpandWhenGoingUpper(string value,string expandValue )
        {
           
            var builder = new StringBuilder();

            var i = 0;

            foreach(var charItem in value.ToCharArray())
            {
                if(i >= 1 && charItem>='A' && charItem<='Z')
                    builder.Append(expandValue);

                if( i == 0)
                    builder.Append(charItem.ToString().ToUpper());
                else if (charItem != '.')
                    builder.Append(charItem);
               
                i += 1;
            }
           
            return builder.ToString();
        
        }

        
        private string FullTypeName(Type type)
        {
            return string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
        }

        public ISessionFactory SessionFactory
        {
            get { return configuration.BuildSessionFactory(); }
        }

        public string ExportSchema()
        {
            var sqlBuilder = new StringBuilder();

            var types = (from type in TypeEnumerator.Types
                         where typeof(Entity).IsAssignableFrom(type)
                           && type != typeof(Entity)
                         select type);

            Configure(types, "");
          
            var export = new NHibernate.Tool.hbm2ddl.SchemaExport(Configuration);
            
            export.Execute(s=> sqlBuilder.AppendLine(s + ";") , false, false);

            return sqlBuilder.ToString();
            
        }

    }
}
