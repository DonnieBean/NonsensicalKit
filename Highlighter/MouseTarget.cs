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
        protected Action onClick;
#if USE_HIGHLIGHTINGSYSTEM
        [SerializeField] protected Highlighter lighter;
#endif
        private void OnMouseEnter()
        {
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOn(Color.cyan);
#endif
        }

        private void OnMouseExit()
        {
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOff();
#endif
        }

        private void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                onClick?.Invoke();
            }
        }
    }
}