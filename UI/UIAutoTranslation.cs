using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RectTransform下应当有一个且仅有一个Image，且锚点完全扩张
/// </summary>
public class UIAutoTranslation : NonsensicalMono
{
    [SerializeField] private RectTransform control;
    [SerializeField] private string signal;
    [SerializeField] private bool horizon;

    private RectTransform rect_self;
    private float offset;

    private float crtPos;
    protected override void Awake()
    {
        base.Awake();
        Subscribe<float>(signal, OnTranslation);
        rect_self = GetComponent<RectTransform>();

        GameObject go2 = Instantiate(control.GetChild(0).gameObject, control);
        GameObject go3 = Instantiate(control.GetChild(0).gameObject, control);
        if (horizon)
        {
            offset = rect_self.rect.width;
            go2.GetComponent<RectTransform>().anchoredPosition -= new Vector2(control.rect.width, 0);
            go3.GetComponent<RectTransform>().anchoredPosition += new Vector2(control.rect.width, 0);
        }
        else
        {
            offset = rect_self.rect.height;
            go2.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, control.rect.height);
            go3.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, control.rect.height);
        }
    }


    Vector2 tempPos = Vector2.zero;
    private void OnTranslation(float value)
    {
        if (value == 0)
        {
            return;
        }
        crtPos += value;
        if (crtPos > offset)
        {
            crtPos -= offset;
        }
        if (crtPos < -offset)
        {
            crtPos += offset;
        }
        float lastPos = crtPos;
        if (value > 0)
        {
            lastPos -= offset;
        }
        if (horizon)
        {
            tempPos.x = lastPos;
        }
        else
        {
            tempPos.y = lastPos;
        }
        control.anchoredPosition = tempPos;
    }
}
