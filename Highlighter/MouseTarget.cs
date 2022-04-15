#if USE_HIGHLIGHTINGSYSTEM
using HighlightingSystem;
#endif
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit.Highlight
{
    public class MouseTarget : NonsensicalMono
    {
#if USE_HIGHLIGHTINGSYSTEM
        [SerializeField] protected Highlighter lighter;
#endif
        [SerializeField] protected string signal;
        protected Action onClick;
        protected bool isHover;
        protected bool isEnter;


        private void OnMouseEnter()
        {
            isHover = true; 
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOn(Color.cyan);
#endif
        }

        private void OnMouseExit()
        {
            isHover = false; 
            isEnter = false;
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOff();
#endif
        }

        private void OnMouseDown()
        {
            if (EventSystemInfoCenter.Instance.MouseNotInUI)
            {
                isEnter = true;
            }
        }

        private void OnMouseUpAsButton()
        {
            if (isEnter)
            {
                isEnter = false;
                Publish(signal);
                onClick?.Invoke();
            }
        }
    }
}
