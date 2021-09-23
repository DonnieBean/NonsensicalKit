using System.Collections.Generic;

namespace NonsensicalKit.Custom
{
    public static class DictionaryPool<T1, T2>
    {
        static Stack<Dictionary<T1, T2>> stack = new Stack<Dictionary<T1, T2>>();

        public static Dictionary<T1, T2> Get()
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            return new Dictionary<T1, T2>();
        }

        public static void Set(Dictionary<T1, T2> list)
        {
            list.Clear();
            stack.Push(list);
        }
    }
}
