using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.UI
{
    public class ListTableManager<ListElement, ElementData> : NonsensicalUI
            where ListElement : ListTableElement<ElementData>
            where ElementData : class
    {
        [SerializeField] protected Transform _group;

        /// <summary>
        /// 是否以group首个子物体作为预制体,为否时应当手动设置_prefab参数
        /// </summary>
        [SerializeField] protected bool _firstPrefab = true;

        [SerializeField] protected bool _customEnd;

        [SerializeField] protected GameObject _prefab;

        protected IEnumerable<ElementData> elementDatas;

        protected Transform endElement;

        protected override void Awake()
        {
            base.Awake();

            if (_firstPrefab)
            {
                _prefab = _group.GetChild(0).gameObject;
                _prefab.gameObject.SetActive(false);
            }
            else
            {
                if (!_prefab)
                {
                    Debug.LogWarning("未设置预制体预制体");
                }
            }
            if (_customEnd)
            {
                endElement = _group.GetChild(_group.childCount - 1);
            }
        }

        protected virtual void UpdateUI(IEnumerable<ElementData> datas)
        {
            elementDatas = datas;

            int crtPos = 0;
            if (_firstPrefab)
            {
                crtPos++;
            }
            int childCount = _group.childCount;
            if (_customEnd)
            {
                childCount--;
            }
            if (datas != null)
            {
                //应用数据链表
                foreach (var item in datas)
                {
                    ListElement crtView;
                    //存在子物体就直接使用，否则则生成一个
                    if (crtPos < childCount)
                    {
                        crtView = _group.GetChild(crtPos).GetComponent<ListElement>();
                    }
                    else
                    {
                        crtView = Instantiate(_prefab, _group).GetComponent<ListElement>();
                    }

                    crtView.gameObject.SetActive(true);
                    crtView.SetValue(item);
                    crtPos++;
                }
            }


            //隐藏剩余未使用的子物体
            for (; crtPos < childCount; crtPos++)
            {
                _group.GetChild(crtPos).gameObject.SetActive(false);
            }

            if (_customEnd)
            {
                endElement.transform.SetAsLastSibling();
            }
        }

        protected virtual void Append(ElementData appendElementData)
        {
            ListElement crtView = null;


            int crtPos = 0;
            if (_firstPrefab)
            {
                crtPos++;
            }
            int childCount = _group.childCount;
            if (_customEnd)
            {
                childCount--;
            }

            for (; crtPos < childCount; crtPos++)
            {
                if (_group.GetChild(crtPos).gameObject.activeSelf == false)
                {
                    crtView = _group.GetChild(crtPos).GetComponent<ListElement>();
                    crtView.transform.SetAsLastSibling();
                    break;
                }
            }

            if (crtView == null)
            {
                crtView = Instantiate(_prefab, _group).GetComponent<ListElement>();
            }

            crtView.gameObject.SetActive(true);
            crtView.SetValue(appendElementData);

            if (_customEnd)
            {
                endElement.transform.SetAsLastSibling();
            }
        }

        protected virtual void Delete(ListElement deleteElement)
        {
            int crtPos = 0;
            if (_firstPrefab)
            {
                crtPos++;
            }
            int childCount = _group.childCount;
            if (_customEnd)
            {
                childCount--;
            }
            for (; crtPos < childCount; crtPos++)
            {
                if (_group.GetChild(crtPos).gameObject.activeSelf != false)
                {
                    if (_group.GetChild(crtPos).GetComponent<ListElement>() == deleteElement)
                    {
                        _group.GetChild(crtPos).gameObject.SetActive(false);
                    }
                }
            }
        }

        protected virtual void Delete(ElementData deleteElementData)
        {
            int crtPos = 0;
            if (_firstPrefab)
            {
                crtPos++;
            }
            int childCount = _group.childCount;
            if (_customEnd)
            {
                childCount--;
            }
            for (; crtPos < childCount; crtPos++)
            {
                if (_group.GetChild(crtPos).gameObject.activeSelf != false)
                {
                    if (_group.GetChild(crtPos).GetComponent<ListElement>().elementData == deleteElementData)
                    {
                        _group.GetChild(crtPos).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
