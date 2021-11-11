using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit
{
    public class DragSpace : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform dragTarget;

        private Vector3 offset;
        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragTarget, eventData.position, eventData.enterEventCamera, out pos);
            offset = dragTarget.position - pos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragTarget, eventData.position, eventData.enterEventCamera, out pos);
            dragTarget.position = pos + offset;
            if (JudgmentUiInScreen(dragTarget, out var v) == false)
            {
                dragTarget.position = v;
            }
        }

        bool JudgmentUiInScreen(RectTransform rect, out Vector3 targetPos)
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            //float power = 800f / screenWidth * 0.5f + 600f / screenHeight * 0.5f;
            float power = 1;

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
                targetPos = dragTarget.position + new Vector3(moveX, moveY, 0);
            }
            return isInView;
        }
    }
}