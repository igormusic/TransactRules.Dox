using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using TransactRules.Core.Entities;
using TransactRules.Core.Utility;

namespace TransactRules.Core.Utilities

{
    public class SessionState:System.ServiceModel.IExtension<OperationContext>

    {
        private  const string SESSION_KEY  = "_session_";

        private DateTime? _valueDate;
        private DateTime? _actionDate;
        private Dictionary<string, Object> _keyValues = new Dictionary<string,object>() ;
        private IUnitOfWork _unitOfWork;

        private static Object lockObject = new Object();

        public DateTime ValueDate {
            get 
            {
                if (!_valueDate.HasValue)
                {
                    _valueDate = ActionDate;
                }

                return _valueDate.GetValueOrDefault();
            }
 
            set
            {
                
                _valueDate = value;
            }
        }
        public DateTime ActionDate 
        {
            get 
            {
                if (!_actionDate.HasValue)
                {
                    _actionDate = DateTime.Today.Date;
                }

                return _actionDate.GetValueOrDefault();
            }
            set 
            {
                _actionDate = value;
            }
        }

        public Dictionary<string, Object> KeyValues {
            get {
                return _keyValues;
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get {
                if (_unitOfWork == null)
                {
                    _unitOfWork = Factory.GetService<IUnitOfWork>();
                }
                return _unitOfWork;
            
            }
            set {
                _unitOfWork = value;
            }
        
        }

        public static SessionState Current
        {
            get
            {
                SessionState result = null;

                lock (lockObject)
                {
                    if (InWebContext())
                    {
                        result = GetSessionStateFromHttpRequest(result);
                    }
                    else if (InWCFContext())
                    {
                        result = GetSessionStateFromWCFOperationContext(result);
                    }
                    else
                    {
                        result = GetSessionStateFromThreadContext(result);
                    }
                }
                
                return result;
            }
        }

        private static SessionState GetSessionStateFromThreadContext(SessionState result)
        {
            result = (SessionState)CallContext.GetData(SESSION_KEY);

            if (result == null)
            {
                result = new SessionState();

                CallContext.SetData(SESSION_KEY, result);
            }
            return result;
        }

        private static SessionState GetSessionStateFromWCFOperationContext(SessionState result)
        {
            result = OperationContext.Current.Extensions.Find<SessionState>();

            if (result == null)
            {
                result = new SessionState();

                OperationContext.Current.Extensions.Add(result);
            }
            return result;
        }

        private static SessionState GetSessionStateFromHttpRequest(SessionState result)
        {
            result = (SessionState)System.Web.HttpContext.Current.Items[SESSION_KEY];

            if (result == null)
            {
                result = new SessionState();

                System.Web.HttpContext.Current.Items.Add(SESSION_KEY, result);
            }
            return result;
        }

        private static bool InWCFContext()
        {
            return OperationContext.Current != null;
        }

        private static bool InWebContext()
        {
            return System.Web.HttpContext.Current != null;
        }

        public void Attach(OperationContext owner)
        {
           
        }

        public void Detach(OperationContext owner)
        {
           
        }
    }
}
