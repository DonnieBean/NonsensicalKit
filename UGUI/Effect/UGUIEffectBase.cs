using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UGUIEffectBase : MonoBehaviour
{

    [Tooltip("如果不赋值，则会在Awake时选择挂载目标")]
    [SerializeField] protected Transform target;

    protected RectTransform rt;

    private bool error = false;
    protected virtual void Awake()
    {
        if (target == null)
        {
            target = transform;
            rt = target.GetComponent<RectTransform>();
            if (rt == null)
            {
                error = true;
                Debug.LogError("目标对象未挂载RectTransform组件");
            }
        }
    }
    public void ShowEffect(int index=0)
    {
        if (!error)
        {
            DoEffect(index);
        }
    }

    protected abstract void DoEffect(int index);
}
