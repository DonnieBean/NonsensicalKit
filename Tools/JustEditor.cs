using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public class JustEditor : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.platform != RuntimePlatform.OSXEditor
                && Application.platform != RuntimePlatform.WindowsEditor
                && Application.platform != RuntimePlatform.LinuxEditor)
            {
                Destroy(gameObject);
            }
        }
    }
}