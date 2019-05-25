using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core
{
    public class Singleton<T> where T : new()
    {
        private static readonly object _locker = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }

                    return _instance;
                }
            }
        }

        public TCore GetCoreComponent<TCore>()
        {
            return IX.Composer.Architecture.Core.GetComponent<TCore>();
        }
    }
}
