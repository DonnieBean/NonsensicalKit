using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class NodeShower : MonoBehaviour
{
    [SerializeField] private SceneNodeMono snm;

    [SerializeField] private List<string> showNodes;

    private void Awake()
    {
        snm.OnSwitchNodeS += OnSwitchNode;
    }
    private void OnSwitchNode(string nodeName)
    {
        gameObject.SetActive(showNodes.Contains(nodeName));
    }
}
