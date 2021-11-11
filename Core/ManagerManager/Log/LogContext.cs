namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 日志上下文，包含一条日志需要传递的所有信息
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