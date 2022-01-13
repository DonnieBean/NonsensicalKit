using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LogicNodeMono))]
public abstract class LogicNodeSwitchBase : NonsensicalMono
{

    protected LogicNodeManager manager;

    protected LogicNodeMono nodeMono;

    protected override void Awake()
    {
        base.Awake();

        manager = LogicNodeManager.Instance;
        nodeMono = GetComponent<LogicNodeMono>();

        nodeMono.OnSwitch.AddListener(OnSwitch);
    }

    protected abstract void OnSwitch(LogicNodeState lns);
}
