using NonsensicalKit.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace NonsensicalKit.Manager
{
    [CustomEditor(typeof(AppConfigManager))]
    public class AppConfigManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AppConfigManager acm = (AppConfigManager)target;
            if (GUILayout.Button("LoadJson"))
            {
                Undo.RecordObject(acm,"LoadJson");
                acm.LoadJson();
            }
        }
    }

}
