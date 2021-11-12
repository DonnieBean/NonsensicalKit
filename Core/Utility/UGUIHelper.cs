using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace NonsensicalKit.Utility
{
    /// <summary>
    /// UGUI工具类
    /// </summary>
    public static class UGUIHelper
    {
        public static void DelayTopping(Scrollbar scrollbar)
        {
            NonsensicalUnityInstance.Instance.DelayDoIt(0, () => { scrollbar.value = 1; });
        }
        public static void InitDropDown(this Dropdown dropDown, Array dropDownType)
        {
            List<Dropdown.OptionData> modelNames = new List<Dropdown.OptionData>();
            foreach (var item in dropDownType)
            {
                modelNames.Add(new Dropdown.OptionData(item.ToString()));
            }
            dropDown.options = modelNames;
        }
        public static void InitDropDown<T>(this Dropdown dropDown) where T : IEnumerable
        {
            List<Dropdown.OptionData> modelNames = new List<Dropdown.OptionData>();
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                modelNames.Add(new Dropdown.OptionData(item.ToString()));
            }
            dropDown.options = modelNames;
        }
    }

}

