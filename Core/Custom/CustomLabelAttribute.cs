using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// ʹ�ֶ���Inspector����ʾ�Զ�������ơ�
    /// </summary>
    public class CustomLabelAttribute : PropertyAttribute
    {
        public string name;

        public CustomLabelAttribute(string name)
        {
            this.name = name;
        }
    }
}

