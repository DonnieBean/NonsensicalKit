using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        RectTransform[] rts = target.GetComponentsInChildren<RectTransform>();
    
        foreach (var item in rts)
        {
            var partentRect = item.parent.GetComponent<RectTransform>().rect;

            var v = item.anchorMin * partentRect.size + item.offsetMin;
            var v2 = item.anchorMax * partentRect.size + item.offsetMax;

            Undo.RecordObject(item, item.name);

            item.anchorMin = v /  partentRect.size;
            item.anchorMax = v2 /  partentRect.size;
            item.offsetMin = Vector2.zero;
            item.offsetMax = Vector2.zero;

            EditorUtility.SetDirty(item);
        }
    }
}
