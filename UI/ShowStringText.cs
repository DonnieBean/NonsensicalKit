using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ShowStringText : NonsensicalMono
{
    [SerializeField] private string signal;

    private Text txt_Self;

    protected override void Awake()
    {
        base.Awake();
        txt_Self = GetComponent<Text>();
        Subscribe<string>(signal,OnChangeText);
    }

    protected void OnChangeText(string str)
    {
        txt_Self.text = str;
    }
}
