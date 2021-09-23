using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NonsensicalKit.Tools
{
    [System.Serializable]
    public class TargetArray
    {
        public GameObject[] Array;
    }

    public class Switcher : NonsensicalMono
    {
        [SerializeField] private TargetArray[] targetArray;

        [SerializeField] private int crtIndex;

        protected override void Awake()
        {
            base.Awake();

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < targetArray.Length; i++)
            {
                foreach (var item in targetArray[i].Array)
                {
                    item.SetActive(i == crtIndex);
                }
            }
        }

        public void ChangeMode(int index)
        {
            foreach (var item in targetArray[crtIndex].Array)
            {
                item.SetActive(false);
            }
            foreach (var item in targetArray[index].Array)
            {
                item.SetActive(true);
            }
            crtIndex = index;
        }

        public void Next()
        {
            if (crtIndex + 1 >= targetArray.Length)
            {
                ChangeMode(0);
            }
            else
            {
                ChangeMode(crtIndex + 1);
            }
        }
    }
}