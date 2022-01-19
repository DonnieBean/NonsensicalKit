using NonsensicalKit.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIPostionConfigData", menuName = "ScriptableObjects/UIPostionConfigData")]
public class UIPostionConfigData : NonsensicalConfigDataBase
{
    public string[] ids;
    public ButtonsParameter[] buttonsParameter;
    public override void CopyForm<T>(T from)
    {
        if (from.ConfigID!=ConfigID)
        {
            return;
        }
        UIPostionConfigData datas = from as UIPostionConfigData;
        this.ids = datas.ids;
        this.buttonsParameter = datas.buttonsParameter;
    }
}

public enum HorizonType
{
    None,
    Left,
    Right
}
public enum VerticalType
{
    None,
    Top,
    Bottom
}


[System.Serializable]
public class ButtonsParameter
{
    public HorizonType horizonType = 0;
    public VerticalType verticalType=0;
    public float distanceHorizon = 100;
    public float distanceVertical = 100;
    public bool configSize=false;
    public float width = 100;
    public float height = 100;
}
