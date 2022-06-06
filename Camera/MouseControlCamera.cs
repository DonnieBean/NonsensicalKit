using NonsensicalKit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit
{
    /// <summary>
    /// 完全鼠标控制摄像机，用于键盘不方便操控的场景，如网页
    /// 有一个虚拟的视点，左键围绕试点旋转，右键以当前正前方为基准向上下左右移动试点
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
        public float zoomSpeed = 0.0001f;

        protected float TargetZoom
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


        [SerializeField] protected  bool canMove=true;

        protected Vector3 tarPos;
        protected float zoom;
        protected float yAngle;
        protected float xAngle;
        protected float targetZoom;

        protected EventSystem crtEventSystem;

        protected InputCenter inputCenter;

        [SerializeField] protected bool checkUI;

        protected bool isOn=true;

        protected bool mouseNotInUI
        {
            get
            {
                if (!checkUI)
                {
                    return true;
                }

                if (crtEventSystem == null)
                {
                    return true;
                }

                return !crtEventSystem.IsPointerOverGameObject();

            }
        }

        private Vector3 startPos;
        private Vector3 startRot;
        private Vector3 startZoom;
        private float trueX;
        private float trueY;

        [SerializeField] private bool autoInit = true;

        protected override void Awake()
        {
            base.Awake();
            startPos = transform.localPosition;
            startRot = swivel.transform.localEulerAngles;
            startZoom = stick.localPosition;

            if (autoInit)
            {
                Init();
            }
        }

        protected virtual void Start()
        {
            crtEventSystem = EventSystem.current;
            inputCenter = InputCenter.Instance;

        }



        public void ResetState()
        {
            transform.localPosition = startPos;
            swivel.transform.localEulerAngles = startRot;
            stick.localPosition = startZoom;

            Init();
        }

        public void Init()
        {
            tarPos = transform.position;
            targetZoom = (stick.localPosition.z / (stickMinZoom + stickMaxZoom));
            zoom = targetZoom;
            yAngle = swivel.transform.localEulerAngles.y;
            trueY = yAngle;
            xAngle = swivel.transform.localEulerAngles.x;
            trueX = xAngle;
        }

        public void Foucs(Transform tsf)
        {
            transform.position = tsf.position;
        }

        protected virtual void Update()
        {
            if (crtEventSystem == null && EventSystem.current != null)
            {
                crtEventSystem = EventSystem.current;
            }

            if (isOn && mouseNotInUI)
            {
                var v = -inputCenter.zoom;
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
                if (inputCenter.mouseLeftKeyHold)
                {
                    AdjustRotation(inputCenter.look);
                }
                if (canMove)
                {
                    if (inputCenter.mouseRightKeyHold)
                    {
                        AdjustPosition(inputCenter.mouseMove);
                    }
                }
            }


        }

        protected void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, tarPos, 0.05f);
            zoom = zoom * 0.95f + TargetZoom * 0.05f;
            float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
            stick.localPosition = new Vector3(0f, 0f, distance);


            transform.position = Vector3.Lerp(transform.position, tarPos, 0.05f);

            trueX = trueX * 0.95f + xAngle * 0.05f;
            trueY = trueY * 0.95f + yAngle * 0.05f;

            swivel.transform.localRotation = Quaternion.Euler(trueX, trueY, 0f);
        }

        /// <summary>
        /// 根据改变量进行缩放
        /// </summary>
        /// <param name="delta"></param>
        protected void AdjustZoom(float delta)
        {
            TargetZoom += delta * zoomSpeed;
        }

        /// <summary>
        /// 根据改变量进行旋转
        /// </summary>
        /// <param name="delta"></param>
        protected void AdjustRotation(Vector2 delta)
        {
            yAngle += delta.x * rotationSpeed * Time.deltaTime;
            //if (yAngle < 0f)
            //{
            //    yAngle += 360f;
            //}
            //else if (yAngle >= 360f)
            //{
            //    yAngle -= 360f;
            //}

            xAngle += delta.y * rotationSpeed * Time.deltaTime;
            //if (xAngle < 0f)
            //{
            //    xAngle += 360f;
            //}
            //else if (xAngle >= 360f)
            //{
            //    xAngle -= 360f;
            //}
        }


        protected void AdjustPosition(Vector2 delta)
        {
            Vector3 direction = swivel.transform.localRotation * new Vector3(delta.x, delta.y, 0f).normalized;
            float damping = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
            float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * damping * Time.deltaTime;

            tarPos += direction * distance;
        }
    }

}

