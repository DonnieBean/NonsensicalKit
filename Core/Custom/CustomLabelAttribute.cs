using UnityEngine;

namespace NonsensicalKit.Custom
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

