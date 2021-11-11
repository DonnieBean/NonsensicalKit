using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 运行平台相关信息
    /// </summary>
    public class PlatformInfo
    {
        static PlatformInfo instance;

        public static PlatformInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlatformInfo();
                }
                return instance;
            }
        }

        public bool isEditor;
        public RuntimePlatform platform;

        private PlatformInfo()
        {
            platform = Application.platform;
            isEditor = platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.LinuxEditor;
        }
    }

}
