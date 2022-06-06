using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������ê���趨Ϊ�պ÷��ϴ�С��λ��
/// </summary>
public class UGUIAutoAnchor : EditorWindow
{
    [MenuItem("Tools/NonsensicalKit/�Զ�����Ӧê��")]
    static void AddComponentToCrtTargetWithChilds()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.Log("δѡ���κζ���");
        }
        else
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                AutoFixed(Selection.gameObjects[i]);
            }
        }
    }
    private static void AutoFixed(GameObject target)
    {
        RectTransform item = target.GetComponent<RectTransform>();
        if (item==null)
        {
            return;
        }

        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(item)      //����Ԥ�������
            || item.GetComponent<ContentSizeFitter>() != null             //��������Ӧ�ߴ����
            || item.parent == null || item.parent.GetComponent<LayoutGroup>() != null)    //������LayoutGroup����Ķ���
        {
            return;
        }
        var partentRT = item.parent.GetComponent<RectTransform>();
        if (partentRT == null)
        {
            return;
        }
        var partentRect = partentRT.rect;

        var v = item.anchorMin * partentRect.size + item.offsetMin;
        var v2 = item.anchorMax * partentRect.size + item.offsetMax;

        if (partentRect.size.x == 0 || partentRect.size.y == 0)
        {
            return;
        }

        Undo.RecordObject(item, item.name);

        item.anchorMin = v / partentRect.size;
        item.anchorMax = v2 / partentRect.size;
        item.offsetMin = Vector2.zero;
        item.offsetMax = Vector2.zero;

        EditorUtility.SetDirty(item);
    }

    ////�����������������׳���
    //private static void AutoFixed(GameObject target)
    //{
    //    RectTransform[] rts = target.GetComponentsInChildren<RectTransform>();

    //    foreach (var item in rts)
    //    {

    //        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(item)      //����Ԥ�������
    //            || item.GetComponent<ContentSizeFitter>()!=null             //��������Ӧ�ߴ����
    //            || item.parent==null||item.parent.GetComponent<LayoutGroup>()!=null)    //������LayoutGroup����Ķ���
    //        {
    //            continue;
    //        }
    //        var partentRT = item.parent.GetComponent<RectTransform>();
    //        if (partentRT==null)
    //        {
    //            continue;
    //        }
    //        var partentRect = partentRT.rect;

    //        var v = item.anchorMin * partentRect.size + item.offsetMin;
    //        var v2 = item.anchorMax * partentRect.size + item.offsetMax;

    //        if (partentRect.size.x==0|| partentRect.size.y == 0)
    //        {
    //            continue;     
    //        }

    //        Undo.RecordObject(item, item.name);

    //        item.anchorMin = v /  partentRect.size;
    //        item.anchorMax = v2 /  partentRect.size;
    //        item.offsetMin = Vector2.zero;
    //        item.offsetMax = Vector2.zero;

    //        EditorUtility.SetDirty(item);
    //    }
    //}
}
