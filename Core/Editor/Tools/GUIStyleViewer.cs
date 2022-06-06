using UnityEngine;
using UnityEditor;

/// <summary>
/// �鿴����GUI���������ñ༭�����߸�ʽ
/// https://blog.csdn.net/u011428080/article/details/106676213
/// </summary>
public class GUIStyleViewer : EditorWindow
{

    Vector2 scrollPosition = new Vector2(0, 0);
    string search = "";
    GUIStyle textStyle;

    private static GUIStyleViewer window;
    [MenuItem("Tools/NonsensicalKit/GUIStyleViewer", false, 10)]
    private static void OpenStyleViewer()
    {
        window = GetWindow<GUIStyleViewer>(false, "����GUIStyle");
    }

    void OnGUI()
    {
        if (textStyle == null)
        {
            textStyle = new GUIStyle("HeaderLabel");
            textStyle.fontSize = 25;
        }

        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Label("������£�", textStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label("Search:");
        search = EditorGUILayout.TextField(search);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
        GUILayout.Label("��ʽչʾ", textStyle, GUILayout.Width(300));
        GUILayout.Label("����", textStyle, GUILayout.Width(300));
        GUILayout.EndHorizontal();


        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var style in GUI.skin.customStyles)
        {
            if (style.name.ToLower().Contains(search.ToLower()))
            {
                GUILayout.Space(15);
                GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                if (GUILayout.Button(style.name, style, GUILayout.Width(300)))
                {
                    EditorGUIUtility.systemCopyBuffer = style.name;
                    Debug.LogError(style.name);
                }
                EditorGUILayout.SelectableLabel(style.name, GUILayout.Width(300));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
    }
}