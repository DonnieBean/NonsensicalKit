using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 控制台输出策略
    /// </summary>
    public class ConsoleLogStrategy : LogStrategy
    {
        public void Init()
        {
        }

        public void Log(LogContext info)
        {
            switch (info.logLevel)
            {
                case LogLevel.DEBUG:
                case LogLevel.INFO:
                    Debug.Log(info.message);
                    break;
                case LogLevel.WARNING:
                    Debug.LogWarning(info.message);
                    break;
                case LogLevel.ERROR:
                case LogLevel.FATAL:
                    Debug.LogError(info.message);
                    break;
                case LogLevel.OFF:
                default:
                    //错误的级别
                    break;
            }
        }

        public void Recycle()
        {
            Debug.Log($"EndLog!DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
    }
}
