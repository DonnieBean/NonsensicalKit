using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态1：放在货位上
/// 状态2：被夹具插入
/// 状态3：被夹具带走
/// 状态4：被放回货位，但夹具仍然接触
/// </summary>
public class ForkMaterials : MonoBehaviour
{
    public string MaterialsName
    {
        get
        {
            return materialsName;
        }
    }
    [SerializeField] private string materialsName;
    [SerializeField] private TriggerPart frontPart;
    [SerializeField] private TriggerPart topPart;
    [SerializeField] private TriggerPart bottomPart;

    private ForkPlacement crtForkPlacement;
    private ForkGrabber crtForkGrabber;

    private int state;

    private ForkPlacement lastPlacement;

    private void Awake()
    {
        frontPart.TriggerEnter += FrontEnter;
        frontPart.TriggerExit += FrontExit;
        topPart.TriggerEnter += TopEnter;
        topPart.TriggerExit += TopExit;
        bottomPart.TriggerEnter += BottomEnter;
        bottomPart.TriggerExit += BottomExit;
    }

    public void Init(ForkPlacement forkPlacement)
    {
        crtForkPlacement = forkPlacement;
        lastPlacement = forkPlacement;
        forkPlacement.SetForkMaterials(this);
        state = 1;
    }

    private void FrontEnter(GameObject go)
    {
        if (state == 1)
        {
            if (go.TryGetComponent<ForkGrabber>(out var v))
            {
                if (crtForkGrabber == null)
                {
                    Debug.Log("FrontEnter：" + go.name);
                    if (v.CanSet(MaterialsName))
                    {
                        crtForkGrabber = v;
                        state = 2;
                    }
                }

            }
        }
    }

    private void FrontExit(GameObject go)
    {
        if (state == 2||state==4)
        {
            if (go.TryGetComponent<ForkGrabber>(out var v))
            {
                Debug.Log("FrontExit：" + go.name);
                if (crtForkGrabber == v)
                {
                    crtForkGrabber = null;
                    state = 1;
                }
            }
        }
    }

    private void TopEnter(GameObject go)
    {
        if (state == 2)
        {
            if (go.TryGetComponent<ForkGrabber>(out var v))
            {
                Debug.Log("TopEnter：" + go.name);
                if (crtForkGrabber == v)
                {
                    if (crtForkGrabber.SetForkMaterials(this))
                    {
                        crtForkPlacement.Clear();
                        crtForkPlacement = null;
                        state = 3;
                    }
                }
            }
        }
    }

    private void TopExit(GameObject go)
    {
        if (go.TryGetComponent<ForkClear>(out var v))
        {
            lastPlacement = null;
        }
    }

    private void BottomEnter(GameObject go)
    {
        if (state == 3)
        {
            if (go.TryGetComponent<ForkPlacement>(out var v))
            {
                if (crtForkPlacement == null&& v!= lastPlacement)
                {
                    Debug.Log("BottomEnter：" + go.name);
                    if (v.SetForkMaterials(this))
                    {
                        crtForkGrabber.Clear();
                        crtForkPlacement = v;
                        lastPlacement = v;
                        state = 4;
                    }
                }

            }
        }
    }

    private void BottomExit(GameObject go)
    {
        
    }
}
