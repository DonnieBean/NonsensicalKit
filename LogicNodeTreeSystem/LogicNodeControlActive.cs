using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �˽ڵ���Ƶ��ж�����
/// </summary>
public enum ControlType
{
    SelfSelect,     //�Լ��Ƿ�ѡ��
    ParentSelect,   //���ڵ��Ƿ�ѡ��
    ChildSelect,    //�ӽڵ��Ƿ�ѡ��
    ParentOrChildSelect //���ڵ���ӽڵ㱻ѡ��
}

/// <summary>
/// �߼��ڵ�������弤��,��������Ҫ���Ƶ�GameObject������
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
