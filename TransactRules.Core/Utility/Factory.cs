using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactRules.Core.Utility
{

    public static class Factory
    {
       

        public static T GetService<T>() where T : class
        {
            return (T)GetService(typeof(T));
        }

        public static object GetService(Type typeOf)
        {
            var implementions = (from type in TypeEnumerator.Types
                                 where typeOf.IsAssignableFrom(type)
                                 && typeOf != type
                                 select type).ToList();

            if (implementions.Count == 0)
            {
                return null;
            }

            return Activator.CreateInstance(implementions[0]);

        }

       
    }
}
