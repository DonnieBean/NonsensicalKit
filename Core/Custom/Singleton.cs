using System;

namespace NonsensicalKit.Custom
{
    public abstract class Singleton<T> where T : class
    {
        private static T instance;
        public static T Instance {
            get
            {
                if (instance ==null)
                {
                    instance = Activator.CreateInstance(typeof(T), true) as T;
                }
                return instance;
            } 
        }

        protected Singleton()
        {
            if (instance == null)
            {
                instance = this as T;
            }
        }
    }
}
