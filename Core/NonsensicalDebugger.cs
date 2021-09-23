using NonsensicalKit.Utility;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace NonsensicalKit
{
    public static class NonsensicalDebugger
    {
        public static void LogWithInfo(object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0
        )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(obj);
            sb.Append($"\r\n{memberName }\r\n({sourceFilePath} :{ sourceLineNumber}");

            Debug.Log(sb.ToString());
        }

        public static void LogOnThreadWithInfo( object obj,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            StringBuilder sb = new StringBuilder();
            if (obj != null)
            {
                sb.Append(obj.ToString());
            }
            else
            {
                sb.Append("obj is null");
            }
            sb.Append($"\r\n{memberName }\r\n({sourceFilePath} :{ sourceLineNumber}");

            NonsensicalUnityInstance.Instance.messages.Enqueue(sb.ToString());

        }
        public static void LogOnThread(params object[] obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StringHelper.GetSetString(obj));

            NonsensicalUnityInstance.Instance.messages.Enqueue(sb.ToString());
        }

        public static void LogOnGUI(params object[] obj)
        {
            NonsensicalUnityInstance.Instance.LogOnGUI(StringHelper.GetSetString(obj), 3);
        }

        public static void Log(params object[] obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StringHelper.GetSetString(obj));

            Debug.Log(sb.ToString());
        }

        public static void LogToFile(string content, string name = null)
        {
            name = name ?? System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Nonsensical", "Log");

            FileHelper.FileAppendWrite(path, name, content);
        }
    }
}
