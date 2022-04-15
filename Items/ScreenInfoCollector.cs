using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit
{
    /// <summary>
    /// 屏幕信息收集
    /// </summary>
    public class ScreenInfoCollector : MonoBehaviour
    {
        [SerializeField] private CanvasScaler cs;
        public float proportion { get; private set; } = 1;

        public int lastHeight { get; private set; } = 0;
        public int lastWidth { get; private set; } = 0;

        private void Update()
        {
            if (lastHeight != Screen.height || lastWidth != Screen.width)
            {
                lastHeight = Screen.height;
                lastWidth = Screen.width;
                UpdateData();
            }
        }

        private void UpdateData()
        {
            switch (cs.uiScaleMode)
            {
                case CanvasScaler.ScaleMode.ConstantPixelSize:
                    {
                        proportion = 1;
                    }
                    break;
                case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                    {
                        switch (cs.screenMatchMode)
                        {
                            case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                                proportion = cs.referenceResolution.x / lastWidth * (1 - cs.matchWidthOrHeight) + cs.referenceResolution.y / lastHeight * cs.matchWidthOrHeight;
                                break;
                            case CanvasScaler.ScreenMatchMode.Expand:
                                proportion = 1;
                                break;
                            case CanvasScaler.ScreenMatchMode.Shrink:
                                proportion = 1;
                                break;
                            default:
                                proportion = 1;
                                break;
                        }
                    }
                    break;
                case CanvasScaler.ScaleMode.ConstantPhysicalSize:
                    proportion = 1;
                    break;
                default:
                    proportion = 1;
                    break;
            }
        }
    }
}