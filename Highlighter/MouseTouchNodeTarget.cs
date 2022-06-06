using HighlightingSystem;
using NonsensicalKit;
using NonsensicalKit.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// 鼠标交互对象控制,当处于父节点时才能进行交互
/// </summary>
[RequireComponent(typeof(Highlighter))]
public class MouseTouchNodeTarget : NonsensicalMono
{
#if USE_HIGHLIGHTINGSYSTEM
    [SerializeField] protected int searchDeep = 1;        //第几级父节点之内被选中时才能交互
    [SerializeField] protected string nodeName;         //点击后跳转的节点名
    private Highlighter lighter;


    private Collider[] colliders;

    private EventSystemInfoCenter esic;
    private LogicNode logicNode;

    private bool isRunning;     //父节点被选择状态时才能交互

    public Func<bool> extraCheck;   //判断是否可以进行交互的额外判断方法

    protected override void Awake()
    {
        base.Awake();
        esic = EventSystemInfoCenter.Instance;
        colliders = GetComponents<Collider>();
        logicNode = LogicNodeManager.Instance.GetNode(nodeName);
        lighter = GetComponent<Highlighter>();
        Subscribe<LogicNode>((int)LogicNodeEnum.SwitchNode, OnSwitchNode);
    }

    private void Start()
    {
        var v = LogicNodeManager.Instance.crtSelectNode;

        OnSwitchNode(v);
    }

    public void OnSwitchNode(LogicNode node)
    {
        if (logicNode == null)
        {
            return;
        }
        LogicNode ln = logicNode;
        int crtDeep = searchDeep;
        isRunning = false;

        //LogManager.Instance.Log(nodeName);
        if (extraCheck == null || extraCheck.Invoke())
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
            lighter.ConstantOff();
        }
    }

    private void OnMouseEnter()
    {
        if (isRunning)
        {
            lighter.ConstantOn(Color.cyan);
        }
    }

    private void OnMouseExit()
    {
        lighter.ConstantOff();
    }

    private void OnMouseUpAsButton()
    {
        if (logicNode != null && esic.MouseNotInUI && isRunning)
        {
            Touch();
        }
    }

    private void Touch()
    {
        LogicNodeManager.Instance.SwitchNode(logicNode);
    }
#endif
}
