using NonsensicalKit.UI;
using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    public class FollowGameobject : MonoBehaviour
    {
        public GameObject target;

        public float scale;

        public Camera mainCamera;

        /// <summary>
        /// ��Ⱦui�����������Canvas����ȾģʽΪOverlayʱ�����ֵӦ��Ϊnull
        /// </summary>
        public Camera RenderCamera;

        public float yOffset;

        public float xOffset;


        [SerializeField] private bool scaleByDistance;
        public float normalDistance;

        public bool back { get; set; }

        private RectTransform rectTransformSelf;

        private void Awake()
        {
            rectTransformSelf = transform.GetComponent<RectTransform>();
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
                Vector3 pos = Vector3.zero;

                if (scaleByDistance && normalDistance != 0)
                {
                    float dis = Vector3.Distance(target.transform.position, mainCamera.transform.position);
                    if (dis > 1f)
                    {
                        transform.localScale = Vector3.one * (normalDistance / dis) * scale;
                    }
                }
                else
                {
                    transform.localScale = Vector3.one * scale;
                }

                pos = mainCamera.WorldToScreenPoint(target.transform.position);
                back = pos.z < 0;

                pos.x += xOffset;
                pos.y += yOffset;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransformSelf, pos, RenderCamera, out Vector3 worldPoint))
                {
                    transform.position = worldPoint;
                }
            }
        }
    }
}
