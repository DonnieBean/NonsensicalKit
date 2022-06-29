using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UGUIEffectBase : MonoBehaviour
{

    [Tooltip("�������ֵ�������Awakeʱѡ�����Ŀ��")]
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
                Debug.LogError("Ŀ�����δ����RectTransform���");
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
