namespace NonsensicalKit.Manager
{
    /// <summary>
    /// ��־�����ģ�����һ����־��Ҫ���ݵ�������Ϣ
    /// </summary>
    public class LogContext
    {
        public LogLevel logLevel;
        public string message;

        public string[] tags;
        public LogContext(LogLevel logLevel,  string message,params string[] tags)
        {
            this.logLevel = logLevel;
            this.message = message;
            this.tags = tags;
        }
    }
}