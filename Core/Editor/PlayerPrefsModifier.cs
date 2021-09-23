using System.IO;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{
    public class PlayerPrefsModifier: EditorWindow
    {
        [MenuItem("Tools/NonsensicalKit/PlayerPrefs修改器")]
        static void ShowWindow()
        {
            GetWindow(typeof(PlayerPrefsModifier));
        }

        private enum DataType{Int,Float, String }
        DataType dataType;
        string prefsName;

        private void OnGUI()
        {
            prefsName = EditorGUILayout.TextField("变量名",prefsName);

            switch (dataType)
            {
                case DataType.Int:
                    //EditorGUILayout.LabelField();
                    break;
                case DataType.Float:
                    break;
                case DataType.String:
                    break;
            }

            if (GUILayout.Button("清空所有PlayerPrefs"))
            {
                PlayerPrefs.DeleteAll();
            }
        }

    }
}
