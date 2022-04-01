using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalControlMuiltObject : NonsensicalMono
{
    [SerializeField] private GameObject[] target;
    [SerializeField] private string signal;

    protected override void Awake()
    {
        base.Awake();

        Subscribe<int>(signal, OnShowTarget);
    }

    private void OnShowTarget(int index)
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].SetActive(i==index);
        }
    }
}
