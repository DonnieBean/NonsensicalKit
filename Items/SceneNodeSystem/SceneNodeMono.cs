using NonsensicalKit;
using System;
using UnityEngine;

public class SceneNodeMono : NonsensicalMono
{
    [SerializeField] private string nodeName;
    public Action<bool> OnSwitchNodeB;
    public Action<string> OnSwitchNodeS;
    public bool isON { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        Subscribe<SceneNode>((uint)SceneNodeEnum.SwitchNode, OnSwitchNode);
    }

    private void Start()
    {
        isON = SceneNodeManager.Instance.CheckState(nodeName);
        OnSwitchNodeB?.Invoke(isON);
    }

    private void OnSwitchNode(SceneNode sn)
    {
        OnSwitchNodeS?.Invoke(sn.NodeName);

        isON = SceneNodeManager.Instance.CheckState(nodeName);
        OnSwitchNodeB?.Invoke(isON);
    }
}
