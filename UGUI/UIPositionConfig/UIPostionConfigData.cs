using NonsensicalKit.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIPostionConfigData", menuName = "ScriptableObjects/UIPostionConfigData")]
public class UIPostionConfigData : NonsensicalConfigDataBase
{
    public UIPositionData configData;
    public override ConfigDataBase GetData()
    {
        return configData;
    }

    public override void SetData(ConfigDataBase cd)
    {
        if (CheckType<UIPositionData>(cd))
        {
            configData = cd as UIPositionData;
        }
    }
}

[System.Serializable]
public class UIPositionData:ConfigDataBase
{
    public string[] ids;
    public ButtonsParameter[] buttonsParameter;
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
