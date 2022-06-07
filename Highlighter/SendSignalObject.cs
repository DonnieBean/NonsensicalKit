using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USE_HIGHLIGHTINGSYSTEM
using HighlightingSystem;
#endif

public class SendSignalObject : NonsensicalMono
{
    [SerializeField] private string signal;


    protected Action OnEnter;
    protected Action OnExit;

#if USE_HIGHLIGHTINGSYSTEM
    [SerializeField] private Highlighter highlighter;
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
#endif

    private void OnMouseDown()
    {
        Publish(signal);
    }
}
