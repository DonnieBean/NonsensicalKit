using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{
    public class PrefsModifier : EditorWindow
    {
        [MenuItem("Tools/NonsensicalKit/Prefs修改器")]
        static void ShowWindow()
        {
            GetWindow(typeof(PrefsModifier));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("清空所有PlayerPrefs"))
            {
                PlayerPrefs.DeleteAll();
            }
            if (GUILayout.Button("清空所有EditorPrefs"))
            {
                EditorPrefs.DeleteAll();
            }
        }
    }
}
