#if USE_HIGHLIGHTINGSYSTEM
using HighlightingSystem;
using UnityEngine;

namespace NonsensicalKit.Highlight
{
    [RequireComponent(typeof(Highlighter))]
    public abstract class NonsensicalHighlighterBase : NonsensicalMono
    {
        public Color TouchColor = Color.cyan;
        public Color SelectColor = Color.yellow;


        private bool isTouch;
        private bool isSelect;

        private Highlighter highlighter;
        protected override void Awake()
        {
            base.Awake();
            highlighter.GetComponent<Highlighter>();
        }

        public virtual void Select()
        {
            isSelect = true;

            highlighter.ConstantOnImmediate(SelectColor);
        }

        public virtual void UnSelect()
        {
            isSelect = false;
            if (isTouch)
            {

                highlighter.ConstantOnImmediate(TouchColor);
            }
            else
            {
                highlighter.ConstantOff();
            }
        }


        public virtual void UnTouch()
        {
            isTouch = false;
            if (!isSelect)
            {
                highlighter.ConstantOff();
            }
        }

        public virtual void Touch()
        {
            isTouch = true;
            if (!isSelect)
            {
                highlighter.ConstantOnImmediate(Color.blue);
            }
        }
    }
}
#endif
