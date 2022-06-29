using NonsensicalKit.UI;
using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 使UI跟随目标对象移动
    /// </summary>
    public class FollowGameobject : MonoBehaviour
    {

        [SerializeField] private Transform target;


        [SerializeField] private float scale = 1;


        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// 渲染ui的摄像机，当Canvas的渲染模式为Overlay时，这个值应当为null
        /// </summary>

        [SerializeField] private Camera RenderCamera;

        [SerializeField] private float yOffset;

        [SerializeField] private float xOffset;


        [SerializeField] private bool scaleByDistance = false;

        [SerializeField] private float normalDistance = 1;

        public bool back { get; set; }

        private RectTransform rectTransformSelf;

        private Vector3 lastTargetPostion;
        private Vector3 lastCameraPostion;
        private Quaternion lastCameraRotation;

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
        Vector3 pos;
        Vector3 targetPosition;
        Vector3 cameraPosition;
        Quaternion cameraRotation;
        private void Update()
        {
            if (target != null)
            {
                targetPosition = target.position;
                cameraPosition = mainCamera.transform.position;
                cameraRotation = mainCamera.transform.rotation;
                if (targetPosition != lastTargetPostion || cameraPosition != lastCameraPostion || cameraRotation != lastCameraRotation)
                {
                    if (scaleByDistance && normalDistance != 0)
                    {
                        float dis = Vector3.Distance(target.position, mainCamera.transform.position);
                        if (dis > 1f)
                        {
                            transform.localScale = Vector3.one * (normalDistance / dis) * scale;
                        }
                    }
                    else
                    {
                        transform.localScale = Vector3.one * scale;
                    }

                    Vector3 pos = mainCamera.WorldToScreenPoint(target.position);
                    back = pos.z < 0;
                    if (!back)
                    {
                        pos.x += xOffset;
                        pos.y += yOffset;
                        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransformSelf, pos, RenderCamera, out Vector3 worldPoint))
                        {
                            transform.position = worldPoint;
                        }
                    }

                    lastTargetPostion = targetPosition;
                    lastCameraPostion = cameraPosition;
                    lastCameraRotation = cameraRotation;
                }
            }
        }

        public void SetTarget(GameObject newTarget)
        {
            target = newTarget.transform;
        }
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
