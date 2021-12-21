using System;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// AppConfig��������������Ӧ�̳д���
    /// </summary>
    public abstract class NonsensicalConfigDataBase : ScriptableObject
    {
        public string ConfigID = "ID" + Guid.NewGuid().ToString().Substring(0, 4);

        public abstract void CopyForm<T>(T from  ) where T: NonsensicalConfigDataBase;
    }
}

