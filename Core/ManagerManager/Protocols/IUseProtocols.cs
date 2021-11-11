using NonsensicalKit;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 需要从数据源获取数据的类继承此接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUseProtocols<T> : ICustomEventHandler
    {
        public void OnReceivedMessage(T value);
    }

}
