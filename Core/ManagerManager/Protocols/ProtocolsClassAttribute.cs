using System;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// ������Դ��ȡ�����ݵķ����л�����Ҫʹ�ô�����
    /// </summary>
    public class ProtocolsClassAttribute : Attribute
    {
        public string key;
        public ProtocolsClassAttribute(string key)
        {
            this.key = key;
        }
    }
}
