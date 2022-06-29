using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ×Ô¶¯Ìí¼Ó¼àÌý
/// </summary>
[RequireComponent(typeof(LogicNodeMono))]
public abstract class LogicNodeSwitchBase : NonsensicalMono
{

    protected LogicNodeManager manager;

    protected LogicNodeMono nodeMono;

    protected override void Awake()
    {
        base.Awake();

        manager = LogicNodeManager.Instance;
        if (TryGetComponent<LogicNodeMono>(out nodeMono))
        {
            nodeMono.OnSwitch.AddListener(OnSwitch);
        }
    }

    protected abstract void OnSwitch(LogicNodeState lns);
}
