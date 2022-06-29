using NonsensicalKit.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LogicNodeTreeAsset))]
public class LogicNodeTreeAssetEditor : Editor
{
    private SerializedProperty idProperty;
    LogicNodeTreeAsset asset;
    NodeTreeEdit<LogicNodeData> nodeTreeEdit;
    void OnEnable()
    {
        asset = (LogicNodeTreeAsset)target;
        if (asset==null)
        {
            return;
        }
        if (asset.GetData()==null)
        {
            return;
        }
        var v = (asset.GetData() as LogicNodeTreeConfigData).root;
        if (v!=null)
        {
            nodeTreeEdit = new NodeTreeEdit<LogicNodeData>((asset.GetData() as LogicNodeTreeConfigData).root);

            nodeTreeEdit.GetHeadString += GetHeaderString;
            nodeTreeEdit.OnDrawElement += DrawElement;
            nodeTreeEdit.OnAddElement += ChildAdd;
            nodeTreeEdit.OnRemoveElement = ChildRemove;
            nodeTreeEdit.OnRemoveSelf = RemoveSelf;
            nodeTreeEdit.elementHeight = 42;

            idProperty = serializedObject.FindProperty("configData.ConfigID");
        }
     
    }

    public override void OnInspectorGUI()
    {
        if ((asset.GetData() as LogicNodeTreeConfigData).root == null)
        {
            return;
        }

        serializedObject.Update();
        if (idProperty!=null)
        {
            EditorGUILayout.PropertyField(idProperty);
        }

        serializedObject.ApplyModifiedProperties();

        if (nodeTreeEdit!=null)
        {
            Rect rect = EditorGUI.IndentedRect(GUILayoutUtility.GetRect(0f, nodeTreeEdit.GetTotalHeight())); 
            
            if (nodeTreeEdit.DrawNodeTree(rect))
            {
                //重要！！！！！！这函数卡了我一天
                EditorUtility.SetDirty(asset);
            }
        }
       
    
    }

    private void ChildAdd(LogicNodeData node)
    {
        var v = new LogicNodeData("newNode", "新节点");
        v.parent = node;
        node.children.Add(v);
    }

    private void ChildRemove(LogicNodeData node)
    {
        node.children.RemoveAt(node.children.Count - 1);
    }

    private void RemoveSelf(LogicNodeData node)
    {
        node.parent.children.Remove(node);
    }

    private string GetHeaderString(LogicNodeData node)
    {
        return node.nodeName;
    }

    private void DrawElement(Rect rect, LogicNodeData node)
    {
        float width = rect.width * 0.5f;
        float height = (rect.height - 2) * 0.5f;
        Rect newRect = new Rect(rect.x, rect.y, width, height);
        GUI.Label(newRect, "NodeName");
        newRect.x += width;
        node.nodeName = GUI.TextField(newRect, node.nodeName);
        newRect.y += height + 2;
        newRect.x -= width;
        GUI.Label(newRect, "AliasName ");
        newRect.x += width;
        node.aliasName = GUI.TextField(newRect, node.aliasName);
    }
}