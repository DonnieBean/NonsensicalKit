using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ĳЩ����ڵ�(�޶�Ӧ��ʵ�����)���Զ���ת
/// </summary>
public class AutoJumpNode : LogicNodeSwitchBase
{
    [SerializeField] private string jumpTarget; //��ת��Ŀ�����Ϊ��ʱ��ת����һ��

    private LogicNode node;

    protected override void Awake()
    {
        base.Awake();

        if (string.IsNullOrEmpty(jumpTarget))
        {
            node = manager.GetNode(nodeMono.NodeName);
            if (node != null && node.ParentNode != null)
            {
                node = node.ParentNode;
            }
        }
        else
        {
            node = manager.GetNode(jumpTarget);
        }
    }

    protected override void OnSwitch(LogicNodeState lns)
    {
        if (node != null && lns.isSelect)
        {
            manager.OnSwitchEnd += () => manager.SwitchNode(node);
        }
    }
}
