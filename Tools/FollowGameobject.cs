using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit.Tools
{

    public class FollowGameobject : MonoBehaviour
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

        private void Awake()
        {
            rectTransform = transform.GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Update()
        {

            if (target != null)
            {
                Vector3 pos = mainCamera.WorldToScreenPoint(target.transform.position);
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