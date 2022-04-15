using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 在非编辑器环境下自动销毁
    /// </summary>
    public class JustEditor : MonoBehaviour
    {
        private void Awake()
        {
            if (PlatformInfo.Instance.isEditor)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}