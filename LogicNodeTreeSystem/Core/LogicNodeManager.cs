using NonsensicalKit.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LogicNodeManager : NonsensicalManagerBase<LogicNodeManager>
{
    public LogicNode crtSelectNode { get; private set; } //当前选择的节点

    private LogicNode root;    //根节点
    private Dictionary<string, LogicNode> dic=new Dictionary<string, LogicNode>();   //所有节点的字典，用于快速查找

    private LogManager log;

    public Action OnSwitchEnd;  //切换后调用一次，然后清空

    protected override void Awake()
    {
        base.Awake();
        log = LogManager.Instance;

        InitSubscribe(2, OnInitStart);
    }

    #region Public Mothod

    public LogicNode GetNode(string nodeName)
    {
        if (dic.ContainsKey(nodeName))
        {
            return dic[nodeName];
        }
        else
        {

            log.LogDebug("未找到节点：" + nodeName);
            return null;
        }
    }

    /// <summary>
    /// 使用别名切换到同级别节点
    /// </summary>
    /// <param name="aliasName"></param>
    /// <returns>切换失败时返回false</returns>
    public bool SwitchSameLevelWithAliasName(string aliasName)
    {
        if (crtSelectNode != null)
        {
            if (crtSelectNode.ParentNode != null)
            {
                foreach (var item in crtSelectNode.ParentNode.ChildNode)
                {
                    if (item.AliasName == aliasName)
                    {
                        SwitchNode(item);
                        return true;
                    }
                }
            }

            //此时意味着当前节点没有父级节点或者同级节点中没有别名相同的对象
            return false;
        }
        else
        {
            log.LogDebug("当前未选择节点时尝试跳转至同级节点");
            return false;
        }
    }

    /// <summary>
    /// 根据别名切换节点，当没有选择的别名时，切换到首个节点
    /// </summary>
    /// <param name="parentNodeName"></param>
    /// <param name="aliasName"></param>
    public void SwitchNodeWithAlias(string parentNodeName, string aliasName)
    {
        if (dic.ContainsKey(parentNodeName))
        {
            LogicNode[] childs = dic[parentNodeName].ChildNode;
            foreach (var item in childs)
            {
                if (item.AliasName == aliasName)
                {
                    SwitchNode(item);
                    return;
                }
            }

            if (childs.Length > 0)
            {
                SwitchNode(childs[0]);
            }
        }
        else
        {
            LogManager.Instance.LogWarning("错误的父节点名称:" + parentNodeName);
        }
    }

    public void SwitchNode(LogicNode node)
    {
        if (dic.ContainsKey(node.NodeName))
        {
            SwitchNodeCheck(node);
        }
        else
        {
            LogManager.Instance.LogWarning("节点不在字典中");
        }
    }

    public void SwitchNode(string nodeName)
    {
        if (dic.ContainsKey(nodeName))
        {
            SwitchNodeCheck(dic[nodeName]);
        }
        else
        {
            LogManager.Instance.LogWarning("错误的节点名称:" + nodeName);
        }
    }

    /// <summary>
    /// 返回上一级
    /// </summary>
    /// <param name="nodeName"></param>
    public bool ReturnPreviousLevel()
    {
        if (crtSelectNode != null)
        {
            if (crtSelectNode.ParentNode == null)
            {
                LogManager.Instance.LogDebug("当前为顶节点，无法返回上一级节点");
                return false;
            }
            SwitchNodeCheck(crtSelectNode.ParentNode);

            return true;
        }
        else
        {
            LogManager.Instance.LogWarning("当前未选择节点");

            return false;
        }
    }

    /// <summary>
    /// 检测自己是否被选中
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public bool CheckState(string nodeName)
    {
        if (crtSelectNode != null && dic.ContainsKey(nodeName))
        {
            LogicNode crt = dic[nodeName];

            if (crt == crtSelectNode)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckStateWithParent(string nodeName)
    {
        if (crtSelectNode != null && dic.ContainsKey(nodeName))
        {
            LogicNode crt = dic[nodeName];

            while (crt != null)
            {
                if (crt == crtSelectNode)
                {
                    return true;
                }
                else
                {
                    crt = crt.ParentNode;
                }
            }
        }
        return false;
    }

    public bool CheckStateWithChild(string nodeName)
    {
        if (crtSelectNode != null && dic.ContainsKey(nodeName))
        {
            LogicNode checkNode = dic[nodeName];

            Queue<LogicNode> nodes = new Queue<LogicNode>();

            nodes.Enqueue(checkNode);

            while (nodes.Count > 0)
            {
                LogicNode crt = nodes.Dequeue();
                if (crt == crtSelectNode)
                {
                    return true;
                }
                else
                {
                    foreach (var item in crt.ChildNode)
                    {
                        nodes.Enqueue(item);
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 更新对应节点的状态并返回是否状态有修改的布尔值
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="nodeState"></param>
    /// <returns></returns>
    public bool UpdateState(string nodeName, LogicNodeState nodeState)
    {
        bool originState1 = nodeState.isSelect;
        bool originState2 = nodeState.parentSelect;
        if (dic.ContainsKey(nodeName))
        {
            LogicNode crt = dic[nodeName];
            nodeState.isSelect = crt == crtSelectNode;

            nodeState.parentSelect = false;
            while (crt != null)
            {
                if (crt == crtSelectNode)
                {
                    nodeState.parentSelect = true;
                    break;
                }
                else
                {
                    crt = crt.ParentNode;
                }
            }
        }
        else
        {
            nodeState.isSelect = false;
            nodeState.parentSelect = false;
        }

        if (originState1 != nodeState.isSelect || originState2 != nodeState.parentSelect)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion


    #region Private Method

    private void OnInitStart()
    {
        if (AppConfigManager.Instance.TryGetConfig<LogicNodeTreeConfigData>(out var v))
        {
            v.OnAfterDeserialize();
            BuildLogicNodeTree(v.root);
            BuildDictionary();
        }
        else
        {
            LogManager.Instance.LogError("未获取到场景节点数据");
        }

    }

    /// <summary>
    /// 构建节点树
    /// </summary>
    /// <param name="datas"></param>
    private void BuildLogicNodeTree(LogicNodeData root)
    {
        Queue<LogicNode> sns = new Queue<LogicNode>();
        Queue<LogicNodeData> snds = new Queue<LogicNodeData>();

        this.root = new LogicNode(root.nodeName, root.aliasName, null, new LogicNode[root.children.Count]);

        sns.Enqueue(this.root);
        snds.Enqueue(root);

        while (sns.Count > 0)
        {
            LogicNode crtNode = sns.Dequeue();
            LogicNodeData crtNodeData = snds.Dequeue();

            for (int i = 0; i < crtNodeData.children.Count; i++)
            {
                int length = crtNodeData.children[i].children.Count;
                var newNode = new LogicNode(crtNodeData.children[i].nodeName, crtNodeData.children[i].aliasName, crtNode, new LogicNode[length]);
                crtNode.ChildNode[i] = newNode;
                if (length > 0)
                {
                    sns.Enqueue(newNode);
                    snds.Enqueue(crtNodeData.children[i]);
                }
            }
        }
    }

    /// <summary>
    /// 构建字典
    /// </summary>
    private void BuildDictionary()
    {
        dic = new Dictionary<string, LogicNode>();

        Queue<LogicNode> nodes = new Queue<LogicNode>();
        nodes.Enqueue(root);

        while (nodes.Count > 0)
        {
            LogicNode crtSN = nodes.Dequeue();
            if (dic.ContainsKey(crtSN.NodeName))
            {
                LogManager.Instance.LogWarning($"节点名称重复:{crtSN.NodeName}");
            }
            else
            {
                dic.Add(crtSN.NodeName, crtSN);
            }

            foreach (var item in crtSN.ChildNode)
            {
                nodes.Enqueue(item);
            }
        }
    }

    private void SwitchNodeCheck(LogicNode node)
    {
        if (node != crtSelectNode)
        {
            LogManager.Instance.LogDebug("切换到节点:" + node.NodeName);
            crtSelectNode = node;
            Publish((int)LogicNodeEnum.SwitchNode, crtSelectNode);
            if (OnSwitchEnd != null)
            {
                //使用临时变量存储并清除原始数据后再执行，防止出现循环调用
                Action onSwitchEnd = OnSwitchEnd;
                OnSwitchEnd = null;
                onSwitchEnd?.Invoke();
            }
        }
    }
    #endregion
}

/// <summary>
/// 树节点，存有父节点和子节点数组的信息
/// </summary>
public class LogicNode
{
    public string NodeName;
    public string AliasName;
    public LogicNode ParentNode;
    public LogicNode[] ChildNode;

    public LogicNode(string nodeName, string aliasName, LogicNode parentNode, LogicNode[] childNode)
    {
        NodeName = nodeName;
        AliasName = aliasName;
        ParentNode = parentNode;
        ChildNode = childNode;
    }
}
