using NonsensicalKit;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static NonsensicalKit.HoldButton;

public class LogicNodeMono : NonsensicalMono
{
    [Serializable]
    public class NodeSwitchEvent : UnityEvent<LogicNodeState> { }

    [SerializeField] private string nodeName;

    [FormerlySerializedAs("onSwitch")]
    [SerializeField]
    private NodeSwitchEvent m_OnSwitch = new NodeSwitchEvent();

    public string NodeName => nodeName;

    private LogicNodeManager manager;

    public NodeSwitchEvent OnSwitch
    {
        get { return m_OnSwitch; }
        set { m_OnSwitch = value; }
    }

    public LogicNodeState nodeState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        manager = LogicNodeManager.Instance;
        nodeState = new LogicNodeState();

        Subscribe<LogicNode>((int)LogicNodeEnum.SwitchNode, OnSwitchNode);
    }
    
    private void Start()
    {
        manager.UpdateState(nodeName, nodeState);
            m_OnSwitch.Invoke(nodeState);
    }

    private void OnSwitchNode(LogicNode sn)
    {
        if (manager.UpdateState(nodeName, nodeState))
        {
            m_OnSwitch.Invoke(nodeState);
        }
    }
}

public class LogicNodeState
{
    public bool isSelect;
    public bool parentSelect;
}