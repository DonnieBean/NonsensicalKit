using System;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// A console to display Unity's debug logs in-game.
    /// </summary>
    public class DebugConsole : MonoBehaviour
    {
        public static DebugConsole Instance;

        #region Inspector Settings

        /// <summary>
        /// The hotkey to show and hide the console window.
        /// </summary>
        public KeyCode toggleKey = KeyCode.BackQuote;
        
        /// <summary>
        /// Whether to open the window by shaking the device (mobile-only).
        /// </summary>
        public bool shakeToOpen = true;

        /// <summary>
        /// The (squared) acceleration above which the window should open.
        /// </summary>
        public float shakeAcceleration = 3f;

        /// <summary>
        /// Whether to only keep a certain number of logs.
        ///
        /// Setting this can be helpful if memory usage is a concern.
        /// </summary>
        public bool restrictLogCount = false;

        /// <summary>
        /// Number of logs to keep before removing old ones.
        /// </summary>
        public int maxLogs = 1000;

        #endregion

        readonly List<LogInfo> logs = new List<LogInfo>();
        Vector2 scrollPosition;
        bool visible;
        bool collapse;

        // Visual elements:

        static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        const string windowTitle = "Console";
        const int margin = 20;
        static readonly GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        static readonly GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

        readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
        Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));


        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                SwitchVisible();
            }
            if (shakeToOpen &&  Input.acceleration.sqrMagnitude > shakeAcceleration)
            {
                SwitchVisible();
            }
        }

        void OnGUI()
        {
            if (!visible)
            {
                return;
            }

            windowRect = GUILayout.Window(123456, windowRect, DrawConsoleWindow, windowTitle);
        }
        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void SwitchVisible()
        {
            visible = !visible;
        }

        /// <summary>
        /// Displays a window that lists the recorded logs.
        /// </summary>
        /// <param name="windowID">Window ID.</param>
        void DrawConsoleWindow(int windowID)
        {
            DrawLogsList();
            DrawToolbar();

            // Allow the window to be dragged by its title bar.
            GUI.DragWindow(titleBarRect);
        }

        /// <summary>
        /// Displays a scrollable list of logs.
        /// </summary>
        void DrawLogsList()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // Iterate through the recorded logs.
            for (var i = 0; i < logs.Count; i++)
            {
                var log = logs[i];

                // Combine identical messages if collapse option is chosen.
                if (collapse && i > 0)
                {
                    var previousMessage = logs[i - 1].message;

                    if (log.message == previousMessage)
                    {
                        continue;
                    }
                }

                GUI.contentColor = logTypeColors[log.logType];

                string logContent = log.ToString();

                if (logContent.Length > 300)
                {
                    logContent = logContent.Substring(0, 300);
                    logContent += "...";
                }

                GUILayout.Label(logContent);
            }



            GUILayout.EndScrollView();

            // Ensure GUI colour is reset before drawing other components.
            GUI.contentColor = Color.white;
        }

        /// <summary>
        /// Displays options for filtering and changing the logs list.
        /// </summary>
        void DrawToolbar()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(clearLabel))
            {
                logs.Clear();
            }


            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Records a log from the log callback.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stackTrace">Trace of where the message came from.</param>
        /// <param name="type">Type of message (error, exception, warning, assert).</param>
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            logs.Add(new LogInfo(message, stackTrace, type, DateTime.Now));
        }

        /// <summary>
        /// Removes old logs that exceed the maximum number allowed.
        /// </summary>
        void TrimExcessLogs()
        {
            if (!restrictLogCount)
            {
                return;
            }

            var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);

            if (amountToRemove == 0)
            {
                return;
            }

            logs.RemoveRange(0, amountToRemove);
        }
    }


    struct LogInfo
    {
        public string message { get; private set; }
        public string stackTrace { get; private set; }
        public LogType logType { get; private set; }
        public DateTime dateTime { get; private set; }

        public LogInfo(string _message, string _staclTrace, LogType _logType, DateTime _dateTime)
        {
            message = _message;
            stackTrace = _staclTrace;
            logType = _logType;
            dateTime = _dateTime;
        }

        public bool CheckType(LogType _logType)
        {
            return logType == _logType;
        }

        public override string ToString()
        {
            return dateTime + " , " + logType.ToString() + "\r\n    " + message + "\r\n    " + stackTrace;
        }
    }
}
