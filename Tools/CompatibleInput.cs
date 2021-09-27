using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// ���ݾ�������µ�����ϵͳ
    /// </summary>
    public class CompatibleInput : MonoSingleton<CompatibleInput>
    {
        #region Mouse
        public Vector3 MousePosition;
        public bool LeftMouseButton;
        public bool RightMouseButton;
        #endregion

        private void Update()
        {
#if USE_INPUT_SYSTEM

#else
            MousePosition = Input.mousePosition;
            LeftMouseButton = Input.GetMouseButton(0);
            RightMouseButton = Input.GetMouseButton(1);
#endif
        }

    }
}