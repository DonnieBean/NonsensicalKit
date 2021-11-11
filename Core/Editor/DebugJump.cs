using NonsensicalKit.Manager;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{
    public class DebugJump
    {
        public static string className = nameof(LogManager) + ".cs";

        [UnityEditor.Callbacks.OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            string stackTrace = GetStackTrace();
            if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains(className))
            {
                Match matches = Regex.Match(stackTrace, @"\(at (.+)\)", RegexOptions.IgnoreCase);
                while (matches.Success)
                {
                    string pathline = matches.Groups[1].Value;

                    if (!pathline.Contains(className))
                    {
                        int splitIndex = pathline.LastIndexOf(":");
                        string path = pathline.Substring(0, splitIndex);
                        line = System.Convert.ToInt32(pathline.Substring(splitIndex + 1));
                        string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                        fullPath += path;
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);

                        return true;
                    }
                    matches = matches.NextMatch();
                }
            }
            return false;
        }

        private static string GetStackTrace()
        {
            var ConsoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");

            if (ConsoleWindowType != null && EditorWindow.focusedWindow.titleContent.text == "Console")
            {
                var activeTextField = ConsoleWindowType.GetField("m_ActiveText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                string activeText = activeTextField.GetValue(EditorWindow.focusedWindow).ToString();

                return activeText;
            }

            return null;
        }
    }
}
