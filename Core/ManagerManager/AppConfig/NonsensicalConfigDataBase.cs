using System;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// AppConfig��������������Ӧ�̳д���
    /// </summary>
    public class NonsensicalConfigDataBase : ScriptableObject
    {
        public string ConfigID = "ID" + Guid.NewGuid().ToString().Substring(0, 4);
    }
}

