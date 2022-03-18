using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SendSignalTouchButton : NonsensicalMono, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private string signal;

  private  bool isHold;
    public  void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;

        Publish(signal,true);
    }

    public  void OnPointerUp(PointerEventData eventData)
    {
        if (isHold)
        {
            isHold = false;
            Publish(signal, false);
        }
    }

    public  void OnPointerExit(PointerEventData eventData)
    {
        if (isHold)
        {
            isHold = false;
            Publish(signal, false);
        }
    }
}
