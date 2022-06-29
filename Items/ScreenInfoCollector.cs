using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit
{
    /// <summary>
    /// 屏幕信息收集
    /// 在屏幕大小改变时发布消息
    /// </summary>
    public class ScreenInfoCollector : NonsensicalMono
    {
        private int lastHeight = 0;
        private int lastWidth = 0;

        private void Update()
        {
            if (lastWidth != Screen.width || lastHeight != Screen.height)
            {
                lastWidth = Screen.width;
                lastHeight = Screen.height;
                Publish("screenSizeChanged", lastWidth, lastHeight);
            }
        }
    }
}