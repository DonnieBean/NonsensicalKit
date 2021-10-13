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


    [SerializeField] private Highlighter highlighter;

    protected Action OnEnter;
    protected Action OnExit;

    private void OnMouseEnter()
    {
#if USE_HIGHLIGHTINGSYSTEM
        highlighter.ConstantOn();
#endif
        OnEnter?.Invoke();
    }

    private void OnMouseExit()
    {
#if USE_HIGHLIGHTINGSYSTEM

        highlighter.ConstantOff();
#endif
        OnExit?.Invoke();
    }

    private void OnMouseDown()
    {
        Publish(signal);
    }
}
