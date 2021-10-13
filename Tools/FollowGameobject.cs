using NonsensicalKit.UI;
using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    public class FollowGameobject : NonsensicalUI
    {
        public GameObject target;

        public Camera mainCamera;

        /// <summary>
        /// 渲染ui的摄像机，当Canvas的渲染模式为Overlay时，这个值应当为null
        /// </summary>
        public Camera RenderCamera;

        RectTransform rectTransform;

        public float yOffset;

        public float xOffset;

        protected override  void Awake()
        {
            base.Awake();
            rectTransform = transform.GetComponent<RectTransform>();
        }

        protected override void Start()
        {
            base.Start();
            Debug.Log(IsShow);
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            return;
            if (target != null)
            {
                Vector3 pos = mainCamera.WorldToScreenPoint(target.transform.position);
                if (pos.z <0)
                {
                    if (IsShow)
                    {
                        Debug.Log(111);
                        CloseSelf();
                    }
                    return;
                }
                else
                {
                    if (!IsShow)
                    {
                        Debug.Log(222);
                        OpenSelf();
                    }
                }
                pos.x += xOffset;
                pos.y += yOffset;
                Vector3 worldPoint;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, pos, RenderCamera, out worldPoint))
                {
                    transform.position = worldPoint;
                }
            }
        }
    }

}