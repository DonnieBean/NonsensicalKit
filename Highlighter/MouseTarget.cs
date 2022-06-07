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
        [SerializeField] protected string signal;
        protected Action onClick;
        protected bool isHover;
        protected bool isEnter;

#if USE_HIGHLIGHTINGSYSTEM
        [SerializeField] protected Highlighter lighter;
        private void OnMouseEnter()
        {
            isHover = true;
            lighter.ConstantOn(Color.cyan);
        }

        private void OnMouseExit()
        {
            isHover = false;
            isEnter = false;
            lighter.ConstantOff();
        }
#endif


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
