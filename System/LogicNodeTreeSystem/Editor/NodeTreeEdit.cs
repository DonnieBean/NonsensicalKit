using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Object = UnityEngine.Object;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine.Pool;

/// <summary>
/// 参考：https://github.com/Unity-Technologies/UnityCsReference/blob/d0fe81a19ce788fd1d94f826cf797aafc37db8ea/Editor/Mono/GUI/ReorderableList.cs
/// 
/// 节点树自定义编辑器，当使用ISerializationCallbackReceiver自定义序列化来避免自循环序列化类报警时，使用这个工具类来绘制节点树
/// </summary>
/// <typeparam name="T"></typeparam>
public class NodeTreeEdit<T> where T : TreeData<T>
{
    public delegate string GetHeaderStringDelegate(T node);
    public GetHeaderStringDelegate GetHeadString;
    public Action<Rect, T> OnDrawElement;
    public Action<Rect, T> OnDrawFooter;
    public Action<T> OnAddElement;
    public Action<T> OnRemoveElement;
    public Action<T> OnRemoveSelf;

    public float elementHeight = 20;
    public float headerHeight = 20;
    public float footerHeight = 20;
    public float levelIndent = 10;    //每级缩进

    private T root;
    private T removeBuffer;

    static Defaults defaults;

    private bool changed;

    public static Defaults defaultBehaviours
    {
        get
        {
            if (defaults == null)
                defaults = new Defaults();

            return defaults;
        }
    }

    public class Defaults
    {
        public GUIContent iconToolbarPlus = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add to the list");
        public GUIContent iconClose = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove self");
        public GUIContent iconToolbarMinus = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove last from the list");
        public readonly GUIStyle headerBackground = "RL Header";
        public readonly GUIStyle footerBackground = "RL Footer";
        public readonly GUIStyle boxBackground = "RL Background";
        public readonly GUIStyle elementBackground = "RL Element";
        public readonly GUIStyle preButton = "RL FooterButton";

        public readonly GUIStyle foldoutHeader = EditorStyles.foldout;
    }

    public NodeTreeEdit(T root)
    {
        this.root = root;
    }

    public float GetTotalHeight()
    {
        return GetHeight(root);
    }

    /// <summary>
    /// 每次绘制都需要多次调用此方法，且同一节点会重复获取多次（先父级后自己），可以将每个节点的高度信息缓存
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private float GetHeight(T node)
    {
        float totalHeiget = 0;
        totalHeiget += headerHeight;
        totalHeiget += elementHeight;
        if (!node.isFoldout)
        {
            return totalHeiget;
        }
        totalHeiget += footerHeight;
        var childs = node.GetChildren();
        if (childs.Count > 0)
        {
            foreach (var item in childs)
            {
                totalHeiget += GetHeight(item);
            }
        }
        return totalHeiget;
    }

    public bool DrawNodeTree(Rect rect)
    {
        changed = false;
        DrawNode(rect, root);

        if (removeBuffer != null)
        {
            OnRemoveSelf?.Invoke(removeBuffer);
            removeBuffer = null;
        }

        return changed;
    }

    private Rect DrawNode(Rect rect, T node)
    {
        Rect headerRect = new Rect(rect.x, rect.y, rect.width, headerHeight);
        DrawListHeader(headerRect, node);
        rect.y += headerHeight;

        if (node.isFoldout)
        {
            if (node.GetChildren().Count > 0)
            {
                float spaceHeight = GetHeight(node) - (headerHeight + footerHeight);


                Rect elementRect = new Rect(rect.x, rect.y, rect.width, spaceHeight);
                DrawElement(elementRect, node);

                Rect listRect = new Rect(rect.x, rect.y + elementHeight, rect.width, spaceHeight);
                DrawListElements(listRect, node);

                rect.y += spaceHeight;
            }
            else
            {
                Rect elementRect = new Rect(rect.x, rect.y, rect.width, elementHeight);
                DrawElement(elementRect, node);
                rect.y += elementHeight;
            }

            Rect footerRect = new Rect(rect.x, rect.y, rect.width, footerHeight);
            DrawListFooter(footerRect, node);
            rect.y += footerHeight;
        }
        else
        {
            Rect elementRect = new Rect(rect.x, rect.y, rect.width, elementHeight);
            DrawElement(elementRect, node);
            rect.y += elementHeight;
        }
        return rect;
    }

    private void DrawListHeader(Rect headerRect, T node)
    {
        if (Event.current.type == EventType.Repaint)
        {
            defaultBehaviours.headerBackground.Draw(headerRect, false, false, false, false);
        }

        int childCount = node.GetChildren().Count;

        string headerString = string.Empty;
        if (GetHeadString != null)
            headerString = GetHeadString(node);
        else
            headerString = "Node";
        if (childCount > 0)
        {

            var toggleRect = headerRect;
            toggleRect.xMax -= 20;
            EditorGUI.BeginChangeCheck();

            node.isFoldout = GUI.Toggle(toggleRect, node.isFoldout, headerString, defaultBehaviours.foldoutHeader);
            if (EditorGUI.EndChangeCheck())
            {
                changed = true;
            }
        }
        else
        {
            GUI.Label(headerRect, headerString);
        }

        headerRect.x = headerRect.xMax - 110;
        GUI.Label(headerRect, "子节点数量：" + childCount);

        headerRect.x += 90;
        headerRect.width = headerRect.height;
        if (GUI.Button(headerRect, defaultBehaviours.iconClose, defaultBehaviours.preButton))
        {
            removeBuffer = node;
            changed = true;
        }
    }

    private void DrawElement(Rect elementRect, T node)
    {
        bool isSelect = true;
        //bool isSelect = selectdObject == node;
        // draw the background in repaint
        if (Event.current.type == EventType.Repaint)
            defaultBehaviours.boxBackground.Draw(elementRect, false, isSelect, isSelect, isSelect);

        elementRect.height = elementHeight;
        elementRect.x += 2;
        elementRect.width -= 4;
        elementRect.y += 2;
        elementRect.height -= 4;
        EditorGUI.BeginChangeCheck();
        OnDrawElement?.Invoke(elementRect, node);
        if (EditorGUI.EndChangeCheck())
        {
            changed = true;
        }
    }

    private void DrawListElements(Rect listRect, T node)
    {
        listRect.xMin += levelIndent;
        foreach (var item in node.GetChildren())
        {
            listRect = DrawNode(listRect, item);
        }
    }

    private void DrawListFooter(Rect footerRect, T node)
    {
        // perform callback or the default footer
        if (OnDrawFooter != null)
        {
            EditorGUI.BeginChangeCheck();
            OnDrawFooter(footerRect, node);
            if (EditorGUI.EndChangeCheck())
            {
                changed = true;
            }
        }
        else
        {
            float leftEdge = footerRect.xMin +5f;
            float rightEdge = leftEdge + 33f;
            footerRect = new Rect(leftEdge, footerRect.y, rightEdge - leftEdge, footerRect.height);
            Rect addRect = new Rect(leftEdge + 4, footerRect.y, 25, 16);
            if (Event.current.type == EventType.Repaint)
            {
                defaultBehaviours.footerBackground.Draw(footerRect, false, false, false, false);
            }

            if (GUI.Button(addRect, defaultBehaviours.iconToolbarPlus, defaultBehaviours.preButton))
            {
                OnAddElement?.Invoke(node);
                changed = true;
            }

        }
    }
}
