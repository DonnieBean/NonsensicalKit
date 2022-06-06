using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchAnime : NonsensicalMono
{
    [SerializeField] private string signal;
    [SerializeField] private Transform control;
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private bool initState;


    private bool isOpen;
    protected override void Awake()
    {
        base.Awake();
        Subscribe<bool>(signal, OnSwitch);
        isOpen = initState;
    }

    private void Update()
    {
        Vector3 targetPos = isOpen ? openPos.position : closePos.position;
        control.position = Vector3.Lerp(control.position, targetPos, 0.1f);
        if (Vector3.Distance(control.position, targetPos) < 1)
        {
            control.position = targetPos;
            enabled = false;
        }
    }

    private void OnSwitch(bool value)
    {
        isOpen = value;
        enabled = true;
    }

}
