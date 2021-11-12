using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NonsensicalKit
{
    [System.Serializable]
    public class TargetArray
    {
        public GameObject[] Array;
    }

    public class Switcher : NonsensicalMono
    {
        [SerializeField] private string signal;
        [SerializeField] private TargetArray[] targetArray;

        [SerializeField] private int crtIndex;

        protected override void Awake()
        {
            base.Awake();

            Init();

            Subscribe<int>(signal, Switch);
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

        public void Switch(int index)
        {
            if (crtIndex>=0&&crtIndex< targetArray.Length)
            {
                foreach (var item in targetArray[crtIndex].Array)
                {
                    item.SetActive(false);
                }
            }
            if (index >= 0&& index < targetArray.Length)
            {
                foreach (var item in targetArray[index].Array)
                {
                    item.SetActive(true);
                }
            }
                
            crtIndex = index;
        }

        public void Next()
        {
            if (crtIndex + 1 >= targetArray.Length)
            {
                Switch(0);
            }
            else
            {
                Switch(crtIndex + 1);
            }
        }
    }
}