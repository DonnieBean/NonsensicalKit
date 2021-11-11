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
        [SerializeField] protected string labelText;
        [SerializeField] protected string signal;
        protected Action onClick;
        protected bool isHover;
        private Vector2 size;
        private Vector2 offset;
        [SerializeField] private float time = 0.5f;
        private float timer;

        protected override void Awake()
        {
            base.Awake();
            size = new Vector2(16 * labelText.Length, 22);
            offset = new Vector2(20, 20);
        }

        private void Update()
        {
            timer += Time.deltaTime;
        }

        private void OnMouseEnter()
        {
            isHover = true;
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOn(Color.cyan);
#endif
        }

        private void OnGUI()
        {
            if (isHover)
            {
                //需要修改字体，默认字体无法在webgl中显示中文
                //GUI.TextArea(new Rect(InputCenter.Instance.mouseScreenPos+offset, size), labelText);
            }
        }

        private void OnMouseExit()
        {
            isHover = false;
#if USE_HIGHLIGHTINGSYSTEM
            lighter.ConstantOff();
#endif
        }

        private void OnMouseDown()
        {
            timer = 0;
        }
        private void OnMouseUpAsButton()
        {
            if (timer < time && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Publish(signal);
                onClick?.Invoke();
            }
        }
    }
}