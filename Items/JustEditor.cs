using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// �ڷǱ༭���������Զ�����
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