using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此节点控制的判断条件
/// </summary>
public enum ControlType
{
    SelfSelect,     //自己是否被选中
    ParentSelect,   //父节点是否被选中
    ChildSelect,    //子节点是否被选中
    ParentOrChildSelect //父节点或子节点被选中
}

/// <summary>
/// 逻辑节点控制物体激活,挂载在需要控制的GameObject对象上
/// </summary>
public class LogicNodeControlActive : NonsensicalMono
{
    [SerializeField] private ControlType controlType;
    [SerializeField] private string nodeName;

    private GameObject controlTarget;

    protected override void Awake()
    {
        base.Awake(); 
        
        controlTarget = gameObject;

        Subscribe<LogicNode>((int)LogicNodeEnum.SwitchNode, OnSwitchNode);
    }

    private void OnEnable()
    {
        if (LogicNodeManager.Instance.crtSelectNode!=null)
        {
            OnSwitchNode(LogicNodeManager.Instance.crtSelectNode);
        }
    }

    private void OnSwitchNode(LogicNode node)
    {
        switch (controlType)
        {
            case ControlType.SelfSelect:
                controlTarget.SetActive(node.NodeName== nodeName);
                break;
            case ControlType.ParentSelect:
                controlTarget.SetActive(LogicNodeManager.Instance.CheckStateWithParent(nodeName));
                break;
            case ControlType.ChildSelect:
                controlTarget.SetActive(LogicNodeManager.Instance.CheckStateWithChild(nodeName));
                break;
            case ControlType.ParentOrChildSelect:
                controlTarget.SetActive(LogicNodeManager.Instance.CheckStateWithParent(nodeName)|| LogicNodeManager.Instance.CheckStateWithChild(nodeName));
                break;
        }
    }
}
