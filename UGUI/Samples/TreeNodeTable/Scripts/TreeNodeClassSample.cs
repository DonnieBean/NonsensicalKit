using NonsensicalKit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodeClassSample : ITreeNodeClass<TreeNodeClassSample>
{
    public string str { get; set; }

    public bool NeedShow { get; set; }
    public bool IsFold { get; set; }
    public bool IsVisible { get; set; }

    public List<TreeNodeClassSample> GetChild()
    {
        return null;
    }
}
