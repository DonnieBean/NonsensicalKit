using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于某些虚拟节点(无对应的实体对象)的自动跳转
/// </summary>
public class AutoJumpNode : LogicNodeSwitchBase
{
    [SerializeField] private string jumpTarget; //跳转的目标对象，为空时跳转到上一级

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
