using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 将对象的锚点设定为刚好符合大小和位置
/// </summary>
public class UGUIAutoAnchor : EditorWindow
{
    [MenuItem("Tools/NonsensicalKit/自动自适应锚点")]
    static void AddComponentToCrtTargetWithChilds()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.Log("未选中任何对象");
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

        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(item)      //跳过预制体对象
            || item.GetComponent<ContentSizeFitter>() != null             //跳过自适应尺寸对象
            || item.parent == null || item.parent.GetComponent<LayoutGroup>() != null)    //跳过被LayoutGroup管理的对象
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

    ////处理所有子物体容易出错
    //private static void AutoFixed(GameObject target)
    //{
    //    RectTransform[] rts = target.GetComponentsInChildren<RectTransform>();

    //    foreach (var item in rts)
    //    {

    //        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(item)      //跳过预制体对象
    //            || item.GetComponent<ContentSizeFitter>()!=null             //跳过自适应尺寸对象
    //            || item.parent==null||item.parent.GetComponent<LayoutGroup>()!=null)    //跳过被LayoutGroup管理的对象
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
