using NonsensicalKit.UI;
using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// ʹUI����Ŀ������ƶ�
    /// </summary>
    public class FollowGameobject : MonoBehaviour
    {

        [SerializeField] private GameObject target;


        [SerializeField] private float scale=1;


        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// ��Ⱦui�����������Canvas����ȾģʽΪOverlayʱ�����ֵӦ��Ϊnull
        /// </summary>

        [SerializeField] private Camera RenderCamera;

        [SerializeField] private float yOffset;

        [SerializeField] private float xOffset;


        [SerializeField] private bool scaleByDistance;

        [SerializeField] private float normalDistance;

        public bool back { get; set; }

        private RectTransform rectTransformSelf;

        private Vector3 lastTargetPostion;
        private Vector3 lastCameraPostion;

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
            if ( target != null)
            {
                Vector3 targetPosition = target.transform.position;
                Vector3 cameraPosition = mainCamera.transform.position;
                if (targetPosition!=lastTargetPostion||cameraPosition!=lastCameraPostion)
                {
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

                    Vector3 pos = mainCamera.WorldToScreenPoint(target.transform.position);
                    back = pos.z < 0;

                    pos.x += xOffset;
                    pos.y += yOffset;
                    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransformSelf, pos, RenderCamera, out Vector3 worldPoint))
                    {
                        transform.position = worldPoint;
                    }
                    lastTargetPostion = targetPosition;
                    lastCameraPostion = cameraPosition;
                }
            }
        }
    }
}
