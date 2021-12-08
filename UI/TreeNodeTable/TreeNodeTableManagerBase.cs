using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.UI
{
    public abstract class TreeNodeTableManagerBase<NodeElement, ElementData> : NonsensicalUI
        where NodeElement : TreeNodeTableElementBase<ElementData>
        where ElementData : class, ITreeNodeClass<ElementData>, new()
    {
        public Transform _group;

        public GameObjectPool op;

        List<ElementData> elementDatas;

        protected override void Awake()
        {
            base.Awake();
            op = new GameObjectPool(_group.GetChild(0).gameObject, OnReset, OnInit, OnInit);

            elementDatas = new List<ElementData>();
        }

        public void OnReset(GameObject go)
        {
            go.transform.SetAsLastSibling();
            go.SetActive(false);
        }
        public void OnInit(GameObject go)
        {
            go.SetActive(true);
        }

        protected virtual void InitTable(IEnumerable<ElementData> datas)
        {
            elementDatas = datas as List<ElementData>;
            for (int i = 1; i < _group.childCount; i++)
            {
                if (_group.GetChild(i).gameObject.activeSelf)
                {
                    op.Store(_group.GetChild(i).gameObject);
                    i--;
                }
            }

            foreach (var item in datas)
            {
                if (item.NeedShow)
                {
                    GameObject crtChild = op.New();
                    crtChild.transform.SetParent(_group);
                    NodeElement crtView = crtChild.GetComponent<NodeElement>();
                    crtView.SetValue(item, 0);
                }
            }
        }

        protected virtual void AddTopNode(ElementData data)
        {
            elementDatas.Add(data);
            GameObject crtChild = op.New();
            crtChild.transform.SetParent(_group);
            NodeElement crtView = crtChild.GetComponent<NodeElement>();
            crtView.SetValue(data, 0);
        }

        protected virtual void RemoveTopNode(NodeElement element)
        {
            if (elementDatas.Contains(element.elementData) == false)
            {
                return;
            }
            elementDatas.Remove(element.elementData);

            if (element.elementData.IsFold==false)
            {
                Fold(element);
            }

            op.Store(element.gameObject);
        }

        protected virtual void Fold(NodeElement ne)
        {
            ne.ChangeFold(true);
            int getIndex = ne.transform.GetSiblingIndex() + 1;

            Stack<int> indexs = new Stack<int>();
            Stack<ElementData> nes = new Stack<ElementData>();

            indexs.Push(0);
            nes.Push(ne.elementData);

            while (indexs.Count > 0)
            {
                int crtIndex = indexs.Pop();
                ElementData crtED = nes.Pop();

                for (; crtIndex < crtED.GetChild().Count; crtIndex++)
                {
                    GameObject crtGO = _group.GetChild(getIndex).gameObject;
                    NodeElement crtNE = crtGO.GetComponent<NodeElement>();

                    if (crtED.GetChild().Contains(crtNE.elementData)==false)
                    {
                        continue;
                    }

                    op.Store(crtGO);
                    if (crtNE.IsFold == false)
                    {
                        indexs.Push(++crtIndex);
                        nes.Push(crtED);
                        indexs.Push(0);
                        nes.Push(crtNE.elementData);
                        break;
                    }
                }
            }
        }

        protected virtual void UnFold(NodeElement ne)
        {
            ne.ChangeFold(false);
            int setIndex = ne.transform.GetSiblingIndex();

            Stack<int> indexs = new Stack<int>();

            Stack<NodeElement> elementDatas = new Stack<NodeElement>();

            indexs.Push(0);
            elementDatas.Push(ne);

            while (indexs.Count > 0)
            {
                int crtIndex = indexs.Pop();

                NodeElement crtNE = elementDatas.Pop();

                int crtLevel = crtNE.Level + 1;

                List<ElementData> childs = crtNE.elementData.GetChild();

                for (; crtIndex < childs.Count; crtIndex++)
                {
                    ElementData childED = childs[crtIndex];

                    GameObject crtChild = op.New();
                    crtChild.transform.SetParent(_group);
                    crtChild.transform.SetSiblingIndex(++setIndex);
                    NodeElement childNE = crtChild.GetComponent<NodeElement>();
                    childNE.SetValue(childED, crtLevel, childED.IsFold);

                    if (childED.IsFold == false)
                    {
                        indexs.Push(++crtIndex);
                        elementDatas.Push(crtNE);
                        indexs.Push(0);
                        elementDatas.Push(childNE);
                        break;
                    }
                }
            }
        }

        protected virtual NodeElement LocateNode(ElementData ed)
        {
            Queue<List<ElementData>> parents = new Queue<List<ElementData>>();

            foreach (var item in elementDatas)
            {
                parents.Enqueue(new List<ElementData>() { item });
            }

            List<ElementData> result = null;

            while (parents.Count > 0)
            {
                List<ElementData> parent = parents.Dequeue();

                ElementData crtED = parent[parent.Count - 1];

                if (crtED == ed)
                {
                    result = parent;
                    break;
                }

                foreach (var item in crtED.GetChild())
                {
                    List<ElementData> crtPath = new List<ElementData>(parent.ToArray());
                    crtPath.Add(item);
                    parents.Enqueue(crtPath);
                }
            }

            if (result == null )
            {
                return null;
            }

            for (int i = 0; i < result.Count; i++)
            {
                foreach (Transform item in _group)
                {
                    NodeElement crtNE = item.GetComponent<NodeElement>();
                    if (crtNE.elementData == result[i])
                    {
                        if (i== result.Count-1)
                        {
                            return crtNE;
                        }

                        if (crtNE.IsFold)
                        {
                            crtNE.ChangeFold(false);
                            UnFold(crtNE);
                        }
                    }
                }
            }

            return null;
        }

        protected virtual void UpdateNode(NodeElement ne)
        {
            if (ne.IsFold==false)
            {
                Fold(ne);
                UnFold(ne);
            }
        }

        protected virtual void Clear()
        {
            for (int i = 0; i < _group.childCount; i++)
            {
                if (_group.GetChild(i).gameObject.activeSelf == true)
                {
                    op.Store(_group.GetChild(i).gameObject);
                    i--;
                }
            }
        }
    }
}
