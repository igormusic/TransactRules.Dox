using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;
using TransactRules.Core.Utilities;

namespace TransactRules.Core.Mock
{
    public class MockUnitOfWork:IUnitOfWork
    {
        

        public MockUnitOfWork()
        {
            
        }

        private IList GetTypeCollection(Type type){
            var baseType = GetBaseType(type);
            var typeName = baseType.FullName;

            if (!SessionState.Current.KeyValues.ContainsKey(typeName))
            {
                Type collectionType = typeof(List<>).MakeGenericType(new Type[] { baseType });
                var newCollection = (IList)Activator.CreateInstance(collectionType);
                SessionState.Current.KeyValues[typeName] = newCollection;
            }

            var collection = (IList) SessionState.Current.KeyValues[typeName];

            return collection;
        }

        private Type GetBaseType(Type type) {
            if (type.BaseType == typeof(object) || type.BaseType == typeof(Entity))
                return type;
            else
                return GetBaseType(type.BaseType);
        }

        public void Create(IEntity obj)
        {
            var collection = GetTypeCollection(obj.GetType());
            obj.Id = collection.Count + 1;
            collection.Add(obj);
        }

        public IEntity GetById(Type type, int id)
        {
            return 
                (from IEntity obj in GetTypeCollection(type)
                where obj.Id == id
                select obj).FirstOrDefault();
        }

        public void Update(IEntity obj)
        {
            var collection = GetTypeCollection(obj.GetType());
            var item = GetById(obj.GetType(), obj.Id);
            collection.Remove(item);
            collection.Add(obj);
        }

        public void Delete(IEntity obj)
        {
            var collection = GetTypeCollection(obj.GetType());
            var item = GetById(obj.GetType(), obj.Id);
            collection.Remove(item);
        }

        public IQuery CreateQuery(Type ofType)
        {
            var collection = GetTypeCollection(ofType);
            var genericType = typeof(MockQuery<>).MakeGenericType(new Type[] { ofType });


            var method =  typeof(System.Linq.Enumerable).GetMethod("OfType").MakeGenericMethod(ofType);

            var underlying = method.Invoke(null, new object[] { collection });

            var query = Activator.CreateInstance(genericType, new Object[] { underlying });

            return (TransactRules.Core.Entities.IQuery)query;
        }

        public Type GetClass(IEntity obj)
        {
            return obj.GetType();
        }
    }
}
