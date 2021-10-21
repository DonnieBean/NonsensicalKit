using NonsensicalKit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit
{
    /// <summary>
    /// ��Ϊ�ļ�
    /// ��һ�����ؽű�������ƽ��
    /// �ڶ���swivel������ת
    /// ������stick����Զ��
    /// ���ļ�Ϊ�����
    /// </summary>
    public class MouseControlCamera : NonsensicalMono
    {
        /// <summary>
        /// ���ᣨ���ӵ���ת��
        /// </summary>
        public Transform swivel;
        /// <summary>
        /// ���ˣ����ӵ���룩
        /// </summary>
        public Transform stick;

        public float stickMinZoom = -1;
        public float stickMaxZoom = -10;
        public float moveSpeedMinZoom = 1;
        public float moveSpeedMaxZoom = 10;
        public float rotationSpeed = 30;
        public float zoomSpeed = 0.001f;

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

        protected Vector3 tarPos;
        protected float zoom;
        protected float yAngle;
        protected float xAngle;
        protected float targetZoom;

        protected EventSystem crtEventSystem;

        protected InputCenter inputCenter;

        [SerializeField] protected bool checkUI;

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

        protected override void Awake()
        {
            base.Awake();
            inputCenter = InputCenter.Instance;
            tarPos = transform.position;
            targetZoom = (stick.localPosition.z / (stickMinZoom + stickMaxZoom));
            zoom = targetZoom;
            yAngle = swivel.transform.localEulerAngles.y;
            xAngle = swivel.transform.localEulerAngles.x;
        }

        public void Foucs(Transform tsf)
        {
            transform.position = tsf.position;
        }

        protected virtual void Start()
        {
            crtEventSystem = EventSystem.current;
        }

        protected virtual void Update()
        {
            if (crtEventSystem == null && EventSystem.current != null)
            {
                crtEventSystem = EventSystem.current;
            }

            if (mouseNotInUI)
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
                if (inputCenter.mouseRightKeyHold)
                {
                    AdjustPosition(inputCenter.mouseMove);
                }
            }


        }

        protected void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, tarPos, 0.05f);
            zoom = zoom * 0.95f + TargetZoom * 0.05f;
            float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
            stick.localPosition = new Vector3(0f, 0f, distance);
        }

        /// <summary>
        /// ���ݸı�����������
        /// </summary>
        /// <param name="delta"></param>
        protected void AdjustZoom(float delta)
        {
            TargetZoom += delta * zoomSpeed;
        }

        /// <summary>
        /// ���ݸı���������ת
        /// </summary>
        /// <param name="delta"></param>
        protected void AdjustRotation(Vector2 delta)
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

        protected void AdjustPosition(Vector2 delta)
        {
            Vector3 direction = swivel.transform.localRotation * new Vector3(delta.x, delta.y, 0f).normalized;
            float damping = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
            float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * damping * Time.deltaTime;

            tarPos += direction * distance;
        }
    }
}