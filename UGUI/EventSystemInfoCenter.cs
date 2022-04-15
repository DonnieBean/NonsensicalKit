using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  EventSystem信息集中处理类
/// </summary>
public class EventSystemInfoCenter : MonoSingleton<EventSystemInfoCenter>
{
    public bool MouseNotInUI
    {
        get
        {
            if (crtEventSystem == null)
            {
                return true;
            }

            return notInUI;
        }
    }


    private bool notInUI;

    private EventSystem crtEventSystem;

    private void Update()
    {
        if (crtEventSystem == null)
        {
            crtEventSystem = EventSystem.current;
            return;
        }
        notInUI = !crtEventSystem.IsPointerOverGameObject();

    }
}
