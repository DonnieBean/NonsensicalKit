using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class SendSignalObject : NonsensicalMono
{
    [SerializeField] private string signal;

    [SerializeField] private Highlighter highlighter;

    protected Action OnEnter;
    protected Action OnExit;

    private void OnMouseEnter()
    {
        highlighter.ConstantOn();
        OnEnter?.Invoke();
    }

    private void OnMouseExit()
    {

        highlighter.ConstantOff();
        OnExit?.Invoke();
    }

    private void OnMouseDown()
    {
        Publish(signal);
    }
}
