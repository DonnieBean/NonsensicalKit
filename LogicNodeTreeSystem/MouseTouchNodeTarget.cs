#if USE_HIGHLIGHTINGSYSTEM
using HighlightingSystem;
#endif
using NonsensicalKit;
using NonsensicalKit.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// ��꽻���������,�����ڸ��ڵ�ʱ���ܽ��н���
/// </summary>
#if USE_HIGHLIGHTINGSYSTEM
[RequireComponent(typeof(Highlighter))]
#endif
public class MouseTouchNodeTarget : NonsensicalMono
{
    [SerializeField] protected int searchDeep = 1;        //�ڼ������ڵ�֮�ڱ�ѡ��ʱ���ܽ���
    [SerializeField] protected string nodeName;         //�������ת�Ľڵ���
#if USE_HIGHLIGHTINGSYSTEM
    private Highlighter lighter;
#endif


    private Collider[] colliders;

    private EventSystemInfoCenter esic;
    private LogicNode logicNode;

    private bool isRunning;     //���ڵ㱻ѡ��״̬ʱ���ܽ���

    public Func<bool> extraCheck;   //�ж��Ƿ���Խ��н����Ķ����жϷ���

    protected override void Awake()
    {
        base.Awake();
        esic = EventSystemInfoCenter.Instance;
        colliders = GetComponents<Collider>();
        logicNode = LogicNodeManager.Instance.GetNode(nodeName);
#if USE_HIGHLIGHTINGSYSTEM
        lighter = GetComponent<Highlighter>();
#endif
        Subscribe<LogicNode>((uint)LogicNodeEnum.SwitchNode, OnSwitchNode);
    }

    private void Start()
    {
        var v = LogicNodeManager.Instance.crtSelectNode;

        OnSwitchNode(v);
    }

    public void OnSwitchNode(LogicNode node)
    {
        if (logicNode==null)
        {
            return;
        }
        LogicNode ln = logicNode;
        int crtDeep = searchDeep;
        isRunning = false;

        //LogManager.Instance.Log(nodeName);
        if (extraCheck==null || extraCheck.Invoke())
        {
            while (ln.ParentNode != null && crtDeep > 0)
            {
                //LogManager.Instance.Log(nodeName,ln.ParentNode.NodeName,node.NodeName);
                if (node == ln.ParentNode)
                {
                    isRunning = true;
                    break;
                }
                ln = ln.ParentNode;
                crtDeep--;
            }
        }
     

        foreach (var item in colliders)
        {
            item.enabled = isRunning;
        }
        if (!isRunning)
        {
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOff();
#endif
        }
    }

    private void OnMouseEnter()
    {
        if (isRunning)
        {
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOn(Color.cyan);
#endif
        }
    }

    private void OnMouseExit()
    {
#if USE_HIGHLIGHTINGSYSTEM
        lighter.ConstantOff();
#endif
    }

    private void OnMouseUpAsButton()
    {
        if (logicNode!=null && esic.MouseNotInUI&&isRunning )
        {
            Touch();
        }
    }

    private void Touch()
    {
        LogicNodeManager.Instance.SwitchNode(logicNode);
    }
}
