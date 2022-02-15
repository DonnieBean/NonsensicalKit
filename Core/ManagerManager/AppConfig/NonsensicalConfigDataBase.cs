using System;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// AppConfig管理类管理对象类应继承此类
    /// </summary>
    public abstract class NonsensicalConfigDataBase : ScriptableObject
    {
        public abstract ConfigDataBase GetData();
        public abstract void SetData(ConfigDataBase cd);
        public virtual void OnSetDataEnd()
        {

        }

        protected bool CheckType<T>(ConfigDataBase cdb) where T : ConfigDataBase
        {
            return cdb.GetType() == typeof(T);
        }
    }

    [Serializable]
    public abstract class ConfigDataBase
    {
        public string ConfigID = "ID" + Guid.NewGuid().ToString().Substring(0, 4);
    }
}
