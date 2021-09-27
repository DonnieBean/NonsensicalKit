using System;
using System.Collections.Generic;

namespace NonsensicalKit
{
    /// <summary>
    /// 在2018.1.1版本中    
    /// 当使用()=>{()=>{}}这种结构嵌套到一定次数时，unity会出现无限加载脚本的bug
    /// 这时可以使用这个类实现相同的效果同时规避bug
    /// </summary>
    public class ActionQueue
    {
        public bool HaveRemaining => actions.Count > 0;

        private Queue<Action> actions;

        public ActionQueue()
        {
            actions = new Queue<Action>();
        }
        public void AddAction(Action action)
        {
            actions.Enqueue(action);
        }
        public void DoNext()
        {
            Action action = actions.Dequeue();
            action?.Invoke();
        }
    }
}