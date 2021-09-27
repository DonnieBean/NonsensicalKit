using NonsensicalKit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit
{
    /// <summary>
    /// 分为四级
    /// 第一级挂载脚本，负责平移
    /// 第二级swivel负责旋转
    /// 第三级stick负责远近
    /// 第四级为摄像机
    /// </summary>
    public class MouseControlCamera : NonsensicalMono
    {
        /// <summary>
        /// 旋轴（与视点旋转）
        /// </summary>
        public Transform swivel;
        /// <summary>
        /// 旋杆（与视点距离）
        /// </summary>
        public Transform stick;

        public float stickMinZoom = -1;
        public float stickMaxZoom = -10;
        public float moveSpeedMinZoom = 1;
        public float moveSpeedMaxZoom = 10;
        public float rotationSpeed = 30;
        public float zoomSpeed = 0.001f;

        private float TargetZoom
        {
            get
            {
                return targetZoom;
            }
            set
            {
                targetZoom = Mathf.Clamp01(value);
            }
        }

        private Vector3 tarPos;
        private float zoom;
        private float yAngle;
        private float xAngle;
        private float targetZoom;

        private EventSystem crtEventSystem;

        private bool leftOn;
        private bool rightOn;

        protected override void Awake()
        {
            base.Awake();
            tarPos = transform.position;
            targetZoom = (stick.localPosition.z / (stickMinZoom + stickMaxZoom));
            zoom = targetZoom;
            yAngle = swivel.transform.localEulerAngles.y;
            xAngle = swivel.transform.localEulerAngles.x;
        }

        private void Start()
        {
            crtEventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && crtEventSystem.IsPointerOverGameObject() == false)
            {
                leftOn = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                leftOn = false;
            }
            if (Input.GetMouseButtonDown(1) && crtEventSystem.IsPointerOverGameObject() == false)
            {
                rightOn = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                rightOn = false;
            }

            if (crtEventSystem == null || crtEventSystem.IsPointerOverGameObject() == false)
            {
                var v = -Input.GetAxisRaw("Mouse ScrollWheel");
                if (v > 0)
                {
                    v = 120;
                }
                else if (v < 0)
                {
                    v = -120;
                }
                if (v != 0)
                {
                    AdjustZoom(v);
                }
                if (leftOn)
                {
                    AdjustRotation(new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")));
                }
                if (rightOn)
                {
                    AdjustPosition(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                }

            }
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, tarPos, 0.05f);
            zoom = zoom * 0.95f + TargetZoom * 0.05f;
            float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
            stick.localPosition = new Vector3(0f, 0f, distance);
        }

        /// <summary>
        /// 根据改变量进行缩放
        /// </summary>
        /// <param name="delta"></param>
        private void AdjustZoom(float delta)
        {
            TargetZoom += delta * zoomSpeed;
        }

        /// <summary>
        /// 根据改变量进行旋转
        /// </summary>
        /// <param name="delta"></param>
        private void AdjustRotation(Vector2 delta)
        {
            yAngle += delta.x * rotationSpeed * Time.deltaTime;
            if (yAngle < 0f)
            {
                yAngle += 360f;
            }
            else if (yAngle >= 360f)
            {
                yAngle -= 360f;
            }

            xAngle += delta.y * rotationSpeed * Time.deltaTime;
            if (xAngle < 0f)
            {
                xAngle += 360f;
            }
            else if (xAngle >= 360f)
            {
                xAngle -= 360f;
            }

            swivel.transform.localRotation = Quaternion.Euler(xAngle, yAngle, 0f);
        }

        private void AdjustPosition(float xDelta, float yDelta)
        {
            Vector3 direction = swivel.transform.localRotation * new Vector3(xDelta, yDelta, 0f).normalized;
            float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
            float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * damping * Time.deltaTime;

            tarPos += direction * distance;
        }
    }
}