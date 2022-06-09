using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePanel : MonoBehaviour
{
    private bool showEscapePanel;
    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape))
        {
            showEscapePanel = true;
        }
    }
    private void OnGUI()
    {
        if (showEscapePanel)
        {
            GUIStyle guiStyle = GUIStyle.none;
            guiStyle.fontSize = 25;
            guiStyle.normal.textColor = Color.white;
            guiStyle.alignment = TextAnchor.MiddleCenter;

            int width = Screen.width;
            int height = Screen.height;
            GUI.Box(new Rect(width * 0.5f - 175, height * 0.5f - 112.5f, 350, 225), "");

            GUI.Label(new Rect(width * 0.5f - 50, height * 0.5f - 100, 100, 50), "�Ƿ��˳�����", guiStyle);
            if (GUI.Button(new Rect(width * 0.5f - 130, height * 0.5f + 50, 60, 30), "ȷ��"))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            if (GUI.Button(new Rect(width * 0.5f + 70, height * 0.5f + 50, 60, 30), "ȡ��"))
            {
                showEscapePanel = false;
            }
        }
    }
}
