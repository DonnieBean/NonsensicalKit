using System.Collections.Generic;

namespace NonsensicalKit.UI
{
    public interface ITreeNodeClass<T>
    {
        public List<T> GetChild();


        public bool NeedShow { get; set; }

        public bool IsFold { get; set; }

        public bool IsVisible { get; set; }
    }
}
