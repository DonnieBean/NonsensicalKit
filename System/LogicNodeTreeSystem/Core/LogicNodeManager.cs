using NonsensicalKit.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LogicNodeManager : NonsensicalManagerBase<LogicNodeManager>
{
    public LogicNode crtSelectNode { get; private set; } //��ǰѡ��Ľڵ�

    private LogicNode root;    //���ڵ�
    private Dictionary<string, LogicNode> dic=new Dictionary<string, LogicNode>();   //���нڵ���ֵ䣬���ڿ��ٲ���

    private LogManager log;

    public Action OnSwitchEnd;  //�л������һ�Σ�Ȼ�����

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

            log.LogDebug("δ�ҵ��ڵ㣺" + nodeName);
            return null;
        }
    }

    /// <summary>
    /// ʹ�ñ����л���ͬ����ڵ�
    /// </summary>
    /// <param name="aliasName"></param>
    /// <returns>�л�ʧ��ʱ����false</returns>
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

            //��ʱ��ζ�ŵ�ǰ�ڵ�û�и����ڵ����ͬ���ڵ���û�б�����ͬ�Ķ���
            return false;
        }
        else
        {
            log.LogDebug("��ǰδѡ��ڵ�ʱ������ת��ͬ���ڵ�");
            return false;
        }
    }

    /// <summary>
    /// ���ݱ����л��ڵ㣬��û��ѡ��ı���ʱ���л����׸��ڵ�
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
            LogManager.Instance.LogWarning("����ĸ��ڵ�����:" + parentNodeName);
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
            LogManager.Instance.LogWarning("�ڵ㲻���ֵ���");
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
            LogManager.Instance.LogWarning("����Ľڵ�����:" + nodeName);
        }
    }

    /// <summary>
    /// ������һ��
    /// </summary>
    /// <param name="nodeName"></param>
    public bool ReturnPreviousLevel()
    {
        if (crtSelectNode != null)
        {
            if (crtSelectNode.ParentNode == null)
            {
                LogManager.Instance.LogDebug("��ǰΪ���ڵ㣬�޷�������һ���ڵ�");
                return false;
            }
            SwitchNodeCheck(crtSelectNode.ParentNode);

            return true;
        }
        else
        {
            LogManager.Instance.LogWarning("��ǰδѡ��ڵ�");

            return false;
        }
    }

    /// <summary>
    /// ����Լ��Ƿ�ѡ��
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
    /// ���¶�Ӧ�ڵ��״̬�������Ƿ�״̬���޸ĵĲ���ֵ
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
            LogManager.Instance.LogError("δ��ȡ�������ڵ�����");
        }

    }

    /// <summary>
    /// �����ڵ���
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
    /// �����ֵ�
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
                LogManager.Instance.LogWarning($"�ڵ������ظ�:{crtSN.NodeName}");
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
            LogManager.Instance.LogDebug("�л����ڵ�:" + node.NodeName);
            crtSelectNode = node;
            Publish((int)LogicNodeEnum.SwitchNode, crtSelectNode);
            if (OnSwitchEnd != null)
            {
                //ʹ����ʱ�����洢�����ԭʼ���ݺ���ִ�У���ֹ����ѭ������
                Action onSwitchEnd = OnSwitchEnd;
                OnSwitchEnd = null;
                onSwitchEnd?.Invoke();
            }
        }
    }
    #endregion
}

/// <summary>
/// ���ڵ㣬���и��ڵ���ӽڵ��������Ϣ
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
