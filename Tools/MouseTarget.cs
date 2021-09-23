using HighlightingSystem;
using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTarget : NonsensicalMono
{
    [SerializeField] protected Highlighter lighter;

    protected Action onClick; 

    private void OnMouseEnter()
    {
        lighter.ConstantOn(Color.cyan);
    }

    private void OnMouseExit()
    {
        lighter.ConstantOff();
    }

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            onClick?.Invoke();
        }
    }
}
