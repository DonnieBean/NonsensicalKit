using UnityEngine;

namespace NonsensicalKit.Highlight
{
#if USE_HIGHLIGHTINGSYSTEM
    public class NonsensicalHighlighterController : NonsensicalMono
    {
        private NonsensicalHighlighterBase lastTouch;
        private NonsensicalHighlighterBase lastSelect;

        protected virtual void Update()
        {
            var v = RaycastTool.instance.GetHit();

            if (v.transform == null || v.transform.GetComponent<NonsensicalHighlighterBase>() == null)
            {
                lastTouch?.UnTouch();
                lastTouch = null;
                if (Input.GetMouseButtonDown(0))
                {
                    lastSelect?.UnSelect();
                    lastSelect = null;
                }
            }
            else
            {
                if (v.transform.GetComponent<NonsensicalHighlighterBase>() != lastTouch)
                {
                    lastTouch?.UnTouch();
                    lastTouch = v.transform.GetComponent<NonsensicalHighlighterBase>();
                    lastTouch.Touch();
                }

                if (Input.GetMouseButtonDown(0) && lastTouch != lastSelect)
                {
                    lastSelect?.UnSelect();
                    lastSelect = lastTouch;
                    lastSelect.Select();
                }
            }
        }
    }
#endif
}
