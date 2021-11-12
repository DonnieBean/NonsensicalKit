using System;

namespace NonsensicalKit
{
    public class TODOException : Exception
    {
        private readonly string _message;
        
        public TODOException()
        {
            _message = "待处理";
        }
        public TODOException(string message)
        {
            _message = message;
        }
        public override string Message
        {
            get
            {
                return "TODO:" + _message;
            }
        }
    }
}
