using NonsensicalKit;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// ��Ҫ������Դ��ȡ���ݵ���̳д˽ӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUseProtocols<T> : ICustomEventHandler
    {
        public void OnReceivedMessage(T value);
    }

}
