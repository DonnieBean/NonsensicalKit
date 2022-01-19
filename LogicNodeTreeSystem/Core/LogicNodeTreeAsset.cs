using NonsensicalKit.Manager;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义序列化存储参考： https://docs.unity3d.com/cn/current/Manual/script-Serialization-Custom.html
/// </summary>
[CreateAssetMenu(fileName = "LogicNodeTree", menuName = "ScriptableObjects/LogicNodeTreeConfigData")]
public class LogicNodeTreeAsset : NonsensicalConfigDataBase, ISerializationCallbackReceiver
{
    [System.NonSerialized]
    public LogicNodeData root;

    public List<SerializableNode> serializedNodes = new List<SerializableNode>();

    public override void CopyForm<T>(T from)
    {
        LogicNodeTreeAsset fromData = from as LogicNodeTreeAsset;
        if (fromData != null)
        {
            serializedNodes = fromData.serializedNodes;

            OnAfterDeserialize();
        }
    }

    public void OnBeforeSerialize()
    {
        if (root == null)
        {
            Debug.Log("根节点为空");
            root = new LogicNodeData("root", "自动根节点");
        }
        serializedNodes.Clear();
        AddNodeToSerializedNodes(root);
    }

    void AddNodeToSerializedNodes(LogicNodeData n)
    {
        var serializedNode = new SerializableNode()
        {
            NodeName = n.nodeName,
            AliasName = n.aliasName,
            childCount = n.children.Count,
        };

        serializedNodes.Add(serializedNode);
        foreach (var child in n.children)
            AddNodeToSerializedNodes(child);
    }

    public void OnAfterDeserialize()
    {
        if (serializedNodes.Count > 0)
        {
            ReadNodeFromSerializedNodes(0, out root);
        }
        else
        {
            Debug.Log("序列化链表为空");
            root = new LogicNodeData("root", "自动根节点");
        }
    }

    int ReadNodeFromSerializedNodes(int index, out LogicNodeData node)
    {
        var serializedNode = serializedNodes[index];
        LogicNodeData newNode = new LogicNodeData()
        {
            nodeName = serializedNode.NodeName,
            aliasName = serializedNode.AliasName,
            children = new List<LogicNodeData>()
        };

        for (int i = 0; i < serializedNode.childCount; i++)
        {
            LogicNodeData childNode;
            index = ReadNodeFromSerializedNodes(++index, out childNode);
            childNode.parent = newNode;
            newNode.children.Add(childNode);
        }
        node = newNode;
        return index;
    }
}

public class LogicNodeData : TreeData<LogicNodeData>
{
    public string nodeName;     //节点名，ID
    public string aliasName;    //别名
    public LogicNodeData parent;
    public List<LogicNodeData> children = new List<LogicNodeData>();

    public LogicNodeData()
    {
    }
    public LogicNodeData(string nodeName, string aliasName)
    {
        this.nodeName = nodeName;
        this.aliasName = aliasName;
    }

    public override List<LogicNodeData> GetChildren()
    {
        return children;
    }

}

// 用于序列化的 Node 类。
[System.Serializable]
public struct SerializableNode
{
    public string NodeName;     //节点名，ID
    public string AliasName;    //别名
    public int childCount;
}
