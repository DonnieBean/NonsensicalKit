using HighlightingSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit.Highlight
{
    public class MouseTarget : NonsensicalMono
    {
        [SerializeField] protected Highlighter lighter;
        [SerializeField] protected string signal;
        protected Action onClick;
        protected bool isHover;
        protected bool isEnter;


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
