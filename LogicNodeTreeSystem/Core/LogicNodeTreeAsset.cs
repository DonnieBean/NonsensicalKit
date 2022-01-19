using NonsensicalKit.Manager;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ������л��洢�ο��� https://docs.unity3d.com/cn/current/Manual/script-Serialization-Custom.html
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
            Debug.Log("���ڵ�Ϊ��");
            root = new LogicNodeData("root", "�Զ����ڵ�");
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
            Debug.Log("���л�����Ϊ��");
            root = new LogicNodeData("root", "�Զ����ڵ�");
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
    public string nodeName;     //�ڵ�����ID
    public string aliasName;    //����
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

// �������л��� Node �ࡣ
[System.Serializable]
public struct SerializableNode
{
    public string NodeName;     //�ڵ�����ID
    public string AliasName;    //����
    public int childCount;
}
