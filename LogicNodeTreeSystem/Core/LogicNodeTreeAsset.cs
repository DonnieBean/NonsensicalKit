using NonsensicalKit.Manager;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogicNodeTree", menuName = "ScriptableObjects/LogicNodeTreeConfigData")]
public class LogicNodeTreeAsset : NonsensicalConfigDataBase
{
    public LogicNodeData[] SceneNodes;

    public override void CopyForm<T>(T from)
    {
        LogicNodeTreeAsset fromData = from as LogicNodeTreeAsset;
        if (fromData != null)
        {
            SceneNodes = fromData.SceneNodes;
        }
    }
}
[System.Serializable]
public class LogicNodeData
{
    public string NodeName;     //节点名，ID
    public string AliasName;    //别名
    public LogicNodeData[] ChildNode;
}