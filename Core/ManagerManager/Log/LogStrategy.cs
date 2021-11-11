using System;
using System.Text;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 日志策略接口
    /// </summary>
    public interface LogStrategy
    {
         abstract void Log(LogContext message);
         abstract void Init();
    }
}
