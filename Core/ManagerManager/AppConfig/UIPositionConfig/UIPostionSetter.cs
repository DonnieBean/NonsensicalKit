using NonsensicalKit;
using NonsensicalKit.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIPostionSetter : NonsensicalMono
{
    [SerializeField] private string configID;

    private RectTransform rt_self;
    private float width;
    private float height;

    protected override void Awake()
    {
        base.Awake();
        rt_self = GetComponent<RectTransform>();

        width = rt_self.rect.width;
        height = rt_self.rect.height;



    }

    private void Start()
    {

        if (NonsensicalRuntimeManager.Instance.allInitComplete)
        {
            OnInitCompleted();
        }
        else
        {
            Subscribe((uint)NonsensicalManagerEnum.AllInitComplete, OnInitCompleted);
        }
    }

    private void OnInitCompleted()
    {
        if (AppConfigManager.Instance.TryGetConfig<UIPostionConfigData>(out var v))
        {
            for (int i = 0; i < v.ids.Length; i++)
            {
                if (configID == v.ids[i])
                {
                    ChangePos(v.buttonsParameter[i]);
                    break;
                }
            }
        }
    }

    private void ChangePos(ButtonsParameter bp)
    {

        switch (bp.horizonType)
        {
            case HorizonType.Left:
                rt_self.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, bp.distanceHorizon, bp.configSize ? bp.width : width);
                break;
            case HorizonType.Right:
                rt_self.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, -bp.distanceHorizon, bp.configSize ? bp.width : width);
                break;
        }

        switch (bp.verticalType)
        {
            case VerticalType.Top:
                rt_self.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -bp.distanceVertical, bp.configSize ? bp.height : height);
                break;
            case VerticalType.Bottom:
                rt_self.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, bp.distanceVertical, bp.configSize ? bp.height : height);
                break;
        }
    }
}
