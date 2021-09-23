using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System;

namespace NonsensicalKit.Editor
{
    /// <summary>
    /// https://blog.csdn.net/zcaixzy5211314/article/details/79549149
    /// </summary>
    public class SetDefaultFont : EditorWindow
    {
        private static Font m_font;
        private static EditorWindow window;

        [MenuItem("CustomTool/设置默认字体")]
        public static void OpenWindow()
        {
            window = GetWindow(typeof(SetDefaultFont));
            window.minSize = new Vector2(500, 300);
            m_font = GetFont();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("选择默认字体");
            EditorGUILayout.Space();
            m_font = (Font)EditorGUILayout.ObjectField(m_font, typeof(Font), true);
            EditorGUILayout.Space();
            if (GUILayout.Button("确定"))
            {
                SetFont(m_font);
                window.Close();
            }
        }

        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.hierarchyChanged += ChangeDefaultFont;
        }

        private static void ChangeDefaultFont()
        {
            Font f = GetFont();

            if (f != null && Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.TryGetComponent<Text>(out var v))
                {
                    v.font = f;
                }
            }
        }

        private static Font GetFont()
        {
            string path = PlayerPrefs.GetString("nk_setDefaultFont_defaultFontPath", "");
            return AssetDatabase.LoadAssetAtPath<Font>(path);
        }

        private static void SetFont(Font f)
        {
            string path = AssetDatabase.GetAssetPath(f);
            PlayerPrefs.SetString("nk_setDefaultFont_DefaultFontPath", path);
        }
    }
}