
namespace NonsensicalKit.UI
{
    public abstract class ListTableElement<ElementData> : NonsensicalUI where ElementData : class
    {
        public ElementData elementData { get; set; }
        public virtual void SetValue(ElementData elementData)
        {
            this.elementData = elementData;
        }
    }

}
