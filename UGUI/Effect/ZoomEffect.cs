using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缩放效果
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
    /// 先瞬间缩小到看不见的大小并移动到鼠标的位置，随后在短时间内插值放大到原始大小并移动到原点位置
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
    /// 短时间缩小到看不见
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
