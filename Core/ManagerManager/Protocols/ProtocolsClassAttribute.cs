using System;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 从数据源获取的数据的反序列化类需要使用此属性
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
