using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.ComponentModel.DataAnnotations;
using TransactRules.Core.Entities;
using TransactRules.Core.Utility;

namespace TransactRules.Core.NHibernateDriver
{
    public class UnitOfWork:IUnitOfWork
    {
        private ISession _session;

        public UnitOfWork()
        {
            _session = SessionManager.OpenSession();
        }

        public ISession Session 
        {
            get 
            {
                return _session;
            }
        
        }

        public void Create(IEntity obj)
        {
            // validate before saving to the database
            Validate(obj);

            _session.Save(obj);
            _session.Flush();
        }

        private static void Validate(IEntity obj)
        {
            var errors = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), errors, true);

            if (!isValid)
            {
                throw new AggregateException(
                    errors.Select((e) => new ValidationException(e.ErrorMessage))
                );
            }
        }

        public IEntity GetById(Type type, int id)
        {

            return (IEntity) _session.Get(type, id);
        }

        public void Update(IEntity obj)
        {
           // validate before saving to the database
            Validate(obj);
            _session.Update(obj);
            _session.Flush();
        }

        public void Delete(IEntity obj)
        {
            _session.Delete(obj);
            _session.Flush();
        }

        public TransactRules.Core.Entities.IQuery CreateQuery(Type ofType)
        {
            var genericType = typeof(Query<>).MakeGenericType(new Type[] {ofType});

            var query = Activator.CreateInstance(genericType, new Object[] { _session });

            return (TransactRules.Core.Entities.IQuery)query;
            
        }

        public Type GetClass(IEntity obj)
        {
            return NHibernate.NHibernateUtil.GetClass(obj);
        }
       
    }
}
