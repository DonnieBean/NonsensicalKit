using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{
    public class PlayerPrefsModifier : EditorWindow
    {
        [MenuItem("Tools/NonsensicalKit/PlayerPrefs修改器")]
        static void ShowWindow()
        {
            GetWindow(typeof(PlayerPrefsModifier));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("清空所有PlayerPrefs"))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
