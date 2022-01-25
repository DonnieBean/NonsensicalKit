using NonsensicalKit.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// ÿ����Ϣ����һ���ȼ���������ΪOFF��,�趨Log�ȼ��󣬽���Log���ڵ����趨�ȼ�����Ϣ
    /// </summary>
    public enum LogLevel
    {
        DEBUG=1,    //��ʾ������Ϣ,�������ڵ��Ե�Debug��Ϣ
        INFO,       //����չʾ��ǰ������ʲô״̬����������ʲô��ʱ����������Ϣ
        WARNING,    //��һ���Ǵ��󣬵���Ӧ������ע��ʱ����Ϣ
        ERROR,      //�����˴��󣬵��ǲ�Ӱ���������ʱ����Ϣ
        FATAL,      //�����˻ᵼ�³����޷��������еĴ���ʱ����Ϣ
        OFF,        //����ʾ������Ϣ
    }

    /// <summary>
    /// ��־·��
    /// </summary>
    public enum LogPath
    {
        Console,
        TxtFile,
    }

    /// <summary>
    /// ��־����
    /// </summary>
    public class LogManager : NonsensicalManagerBase<LogManager>
    {
        private readonly Dictionary<LogPath, LogStrategy> _strategies = new Dictionary<LogPath, LogStrategy>()
        {
            {LogPath.Console,new ConsoleLogStrategy() },
            {LogPath.TxtFile,new TxtFileLogStrategy() }
        };

        private LogLevel logLevel;
        private LogPath[] logPath;
        private bool logDateTime;
        private bool logClassInfo;
        private PlatformInfo platformInfo;

        private StringBuilder sb = new StringBuilder();
        protected override void Awake()
        {
            base.Awake();
            platformInfo = PlatformInfo.Instance;
            InitSubscribe(1, OnInitStart);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var item in _strategies)
            {
                item.Value.Recycle();
            }
        }

        protected  void OnInitStart()
        {
            foreach (var item in _strategies)
            {
                item.Value.Init();
            }

            if (AppConfigManager.Instance.TryGetConfig<NonsensicalManagerConfigData>(out var v))
            {
                if (platformInfo.isEditor)
                {
                    logLevel = v.EditorLogLevel;
                    logPath = v.EditorLogPaths;
                    logDateTime = v.EditorLogDateTime;
                    logClassInfo = false;
                }
                else
                {

                    logLevel = v.EditorLogLevel;
                    logPath = v.EditorLogPaths;
                    logDateTime = v.BuildLogDateTime;
                    logClassInfo = v.BuildLogClassInfo;
                }
            }
            else
            {
                if (platformInfo.isEditor)
                {
                    logLevel = LogLevel.DEBUG;
                    logPath = new LogPath[] { LogPath.Console };
                }
                else
                {
                    logLevel = LogLevel.INFO;
                    logPath = new LogPath[0];
                }
            }
            Log(new LogContext(LogLevel.INFO, $"StartLog!DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\nDevice Model:{SystemInfo.deviceModel},Device Name:{ SystemInfo.deviceName},Operating System:{SystemInfo.operatingSystem}"));

        }
        public  void Log(params object[] obj)
        {
            if (platformInfo.isEditor)
            {
                Debug.Log(StringHelper.GetSetString(obj));
            }
        }

        private void Log(LogContext logInfo)
        {
            if (logInfo.logLevel >= logLevel)
            {
                foreach (var item in logPath)
                {
                    _strategies[item].Log(logInfo);
                }
            }
        }

        public void LogDebug(object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            sb.Clear();
            sb.Append("Debug:");
            if (obj != null)
            {
                sb.AppendLine(obj.ToString());
            }
            else
            {
                sb.AppendLine("null");
            }
            if (logDateTime)
            {
                sb.AppendLine($"DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            if (logClassInfo)
            {
                sb.AppendLine($"{memberName }(at {sourceFilePath} :{ sourceLineNumber})");
            }
            sb.Remove(sb.Length-1,1);
            Log(new LogContext(LogLevel.DEBUG, sb.ToString()));
        }

        public void LogInfo(object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            sb.Clear();
            sb.Append("Info:");
            if (obj != null)
            {
                sb.AppendLine(obj.ToString());
            }
            else
            {
                sb.AppendLine("null");
            }
            if (logDateTime)
            {
                sb.AppendLine($"DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            if (logClassInfo)
            {
                sb.AppendLine($"{memberName }(at {sourceFilePath} :{ sourceLineNumber})");
            }
            sb.Remove(sb.Length - 1, 1);
            Log(new LogContext(LogLevel.INFO, sb.ToString()));
        }

        public void LogWarning(object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            sb.Clear();
            sb.Append("Warning:");
            if (obj != null)
            {
                sb.AppendLine(obj.ToString());
            }
            else
            {
                sb.AppendLine("null");
            }
            if (logDateTime)
            {
                sb.AppendLine($"DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            if (logClassInfo)
            {
                sb.AppendLine($"{memberName }(at {sourceFilePath} :{ sourceLineNumber})");
            }
            sb.Remove(sb.Length - 1, 1);
            Log(new LogContext(LogLevel.WARNING, sb.ToString()));
        }

        public void LogError(object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            sb.Clear();
            sb.Append("Error:");
            if (obj != null)
            {
                sb.AppendLine(obj.ToString());
            }
            else
            {
                sb.AppendLine("null");
            }
            if (logDateTime)
            {
                sb.AppendLine($"DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            if (logClassInfo)
            {
                sb.AppendLine($"{memberName }(at {sourceFilePath} :{ sourceLineNumber})");
            }
            sb.Remove(sb.Length - 1, 1);
            Log(new LogContext(LogLevel.ERROR, sb.ToString()));
        }

        public void LogFatal(object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            sb.Clear();
            sb.Append("Fatal:");
            if (obj != null)
            {
                sb.AppendLine(obj.ToString());
            }
            else
            {
                sb.AppendLine("null");
            }
            if (logDateTime)
            {
                sb.AppendLine($"DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            if (logClassInfo)
            {
                sb.AppendLine($"{memberName }(at {sourceFilePath} :{ sourceLineNumber})");
            }
            sb.Remove(sb.Length - 1, 1);
            Log(new LogContext(LogLevel.FATAL, sb.ToString()));
        }
    }
}
