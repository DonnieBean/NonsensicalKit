using System;
using UnityEngine;

namespace NonsensicalKit.Tools
{
    public class OnGUIButtonGroup
    {
        private float startX;
        private float startY;

        private float width;
        private float height;

        private int fontSize;

        private float intervalX;
        private float intervalY;

        private bool verticalFirst;
        private int countLimit;

        private int count;
        private float crtX;
        private float crtY;

        public OnGUIButtonGroup(float startX = 10, float startY = 10, float width = 200, float height = 40, int fontSize = 25, float intervalX = 10, float intervalY = 10, bool verticalFirst = true, int countLimit = -1)
        {
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
            this.fontSize = fontSize;
            this.intervalX = intervalX;
            this.intervalY = intervalY;
            this.verticalFirst = verticalFirst;
            this.countLimit = countLimit;
        }

        public void Reset()
        {
            GUI.skin.button.fontSize = fontSize;
            count = 0;
            crtX = startX;
            crtY = startY;
        }

        public void AddButton(string text, Action action)
        {
            Rect rect = new Rect(crtX, crtY, width, height);
            if (GUI.Button(rect, text))
            {
                action?.Invoke();
            }

            count++;
            if (verticalFirst)
            {
                if (countLimit != -1 && count % countLimit == 0)
                {
                    int num = count / countLimit;
                    crtX = startX + num * intervalX + (num - 1) * width;
                    crtY = startY;
                }
                else
                {
                    crtY += intervalY + height;
                }
            }
            else
            {
                if (countLimit != -1 && count % countLimit == 0)
                {
                    int num = count / countLimit;
                    crtX = startX;
                    crtY = startY + num * intervalY + (num - 1) * height;
                }
                else
                {
                    crtX += intervalX + width;
                }
            }
        }
    }
}