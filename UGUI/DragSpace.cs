using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NonsensicalKit
{
    /// <summary>
    /// ��ui������������ק�˶�ͬʱ��֤�����ܳ���Ļ
    /// </summary>
    public class DragSpace : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private CanvasScaler canvasScaler;

        //��canvasScaler�޷�ֱ�Ӹ�ֵʱ����Ԥ�����У�������Ҫ�ֶ���д�����ֵ
        [SerializeField] private CanvasScaler.ScaleMode mode;
        [SerializeField] private Vector2 resolution;
        [SerializeField] private float match;

        private RectTransform rt_Self;

        //��갴��ʱ����ק�����ƫ��ֵ
        private Vector3 offset;

        private void Awake()
        {
            rt_Self = GetComponent<RectTransform>();

            if (canvasScaler != null)
            {
                mode = canvasScaler.uiScaleMode;
                resolution = canvasScaler.referenceResolution;
                match = canvasScaler.matchWidthOrHeight;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt_Self, eventData.position, eventData.enterEventCamera, out pos);
            offset = rt_Self.position - pos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt_Self, eventData.position, eventData.enterEventCamera, out pos);
            rt_Self.position = pos + offset;
            if (JudgmentUiInScreen(rt_Self, out var v) == false)
            {
                rt_Self.position = v;
            }
        }

        /// <summary>
        /// ���rect�Ƿ�����Ļ��
        /// ʹrect��positionΪtargetPosʱ��������Ļ��
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private bool JudgmentUiInScreen(RectTransform rect, out Vector3 targetPos)
        {
            if (mode == CanvasScaler.ScaleMode.ConstantPixelSize)
            {
                int screenWidth = Screen.width;
                int screenHeight = Screen.height;

                targetPos = Vector3.zero;
                bool isInView = false;
                float moveX = 0;
                float moveY = 0;
                float realW = rect.sizeDelta.x;
                float realH = rect.sizeDelta.y;

                Vector3 worldPos = rect.transform.position;
                float leftX = worldPos.x - realW / 2;
                float rightX = worldPos.x + realW / 2;
                float downY = worldPos.y - realH / 2;
                float topY = worldPos.y + realH / 2;

                if (leftX >= 0 && rightX <= screenWidth && downY >= 0 && topY <= screenHeight)
                {
                    isInView = true;
                }
                else
                {
                    if (leftX < 0)
                    {
                        moveX = -leftX;
                    }
                    else if (rightX > screenWidth)
                    {
                        moveX = screenWidth - rightX;
                    }
                    if (downY < 0)
                    {
                        moveY = -downY;
                    }
                    else if (topY > screenHeight)
                    {
                        moveY = screenHeight - topY;
                    }
                    targetPos = rt_Self.position + new Vector3(moveX, moveY, 0);
                }
                return isInView;
            }
            else if (mode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                //Ĭ��ʹ��MatchWidthOrHeight
                int screenWidth = Screen.width;
                int screenHeight = Screen.height;
                float power = resolution.x / screenWidth * match + resolution.y / screenHeight * (1 - match);

                targetPos = Vector3.zero;
                bool isInView = false;
                float moveX = 0;
                float moveY = 0;
                float realW = rect.sizeDelta.x / power;
                float realH = rect.sizeDelta.y / power;

                Vector3 worldPos = rect.transform.position;
                float leftX = worldPos.x - realW / 2;
                float rightX = worldPos.x + realW / 2;
                float downY = worldPos.y - realH / 2;
                float topY = worldPos.y + realH / 2;

                if (leftX >= 0 && rightX <= screenWidth && downY >= 0 && topY <= screenHeight)
                {
                    isInView = true;
                }
                else
                {
                    if (leftX < 0)
                    {
                        moveX = -leftX;
                    }
                    else if (rightX > screenWidth)
                    {
                        moveX = screenWidth - rightX;
                    }
                    if (downY < 0)
                    {
                        moveY = -downY;
                    }
                    else if (topY > screenHeight)
                    {
                        moveY = screenHeight - topY;
                    }
                    targetPos = rt_Self.position + new Vector3(moveX, moveY, 0);
                }
                return isInView;
            }
            else
            {
                targetPos = rt_Self.position;
                return true;
            }
        }
    }
}