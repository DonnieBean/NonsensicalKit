using NonsensicalKit;
using NonsensicalKit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodeTableElementSample : TreeNodeTableElementBase<TreeNodeClassSample>
{
    protected override void OnNameClick()
    {
        NonsensicalUnityInstance.Instance.LogOnGUI(elementData.str);
    }
}
