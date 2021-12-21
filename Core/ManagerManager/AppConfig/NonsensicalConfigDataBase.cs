using System;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// AppConfig管理类管理对象类应继承此类
    /// </summary>
    public abstract class NonsensicalConfigDataBase : ScriptableObject
    {
        public string ConfigID = "ID" + Guid.NewGuid().ToString().Substring(0, 4);

        public abstract void CopyForm<T>(T from  ) where T: NonsensicalConfigDataBase;
    }
}

