using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ч��
/// </summary>
public class ZoomEffect : UGUIEffectBase
{
    [SerializeField] private float effectTime = 0.5f;

    private Vector3 originSize;
    private Vector3 originPos;

    protected override void Awake()
    {
        base.Awake();
        originSize = target.localScale;
        originPos = rt.anchoredPosition;
    }

    protected override void DoEffect(int index)
    {
        StopAllCoroutines();
        switch (index)
        {
            default:
            case 0:
                StartCoroutine(DoEnlarge());
                break;
            case 1:
                StartCoroutine(DoReduced());
                break;
        }
    }

    /// <summary>
    /// ��˲����С���������Ĵ�С���ƶ�������λ�ã�����ڶ�ʱ���ڲ�ֵ�Ŵ�ԭʼ��С���ƶ���ԭ��λ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoEnlarge()
    {
        target.localScale = Vector3.one * 0.001f;
        target.position = Input.mousePosition;
        float timer = 0;

        while (timer < effectTime)
        {
            yield return null;
            timer += Time.deltaTime;

            target.localScale = originSize * (timer / effectTime);
            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, originPos, timer / effectTime);
        }
        target.localScale = originSize;
        rt.anchoredPosition = originPos;
    }

    /// <summary>
    /// ��ʱ����С��������
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoReduced()
    {
        target.localScale = originPos;
        float timer = 0;

        while (timer < effectTime)
        {
            yield return null;
            timer += Time.deltaTime;

            target.localScale = originSize * (1-timer / effectTime);
        }
        target.localScale = Vector3.one * 0.001f; 
    }
}
