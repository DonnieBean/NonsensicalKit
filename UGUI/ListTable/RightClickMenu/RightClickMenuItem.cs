using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenuItem 
{
    public string spriteName ;
    public string text;
    public Action clickAction;

    public RightClickMenuItem(string spriteName, string text, Action clickAction)
    {
        this.spriteName = spriteName;
        this.text = text;
        this.clickAction = clickAction;
    }
}
