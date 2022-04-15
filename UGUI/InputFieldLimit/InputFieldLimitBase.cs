using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public abstract class InputFieldLimitBase : NonsensicalMono
{
    [SerializeField] protected string defaultValue;

    protected InputField ipf_self;

    [Serializable]
    public class EndEditEvent : UnityEvent<string> { }
    [FormerlySerializedAs("onEndEdit")]
    [SerializeField]
    private EndEditEvent m_OnEndEdit = new EndEditEvent();
    public EndEditEvent OnEndEdit
    {
        get { return m_OnEndEdit; }
        set { m_OnEndEdit = value; }
    }

    protected override void Awake()
    {
        base.Awake();

        ipf_self = GetComponent<InputField>();
        ipf_self.text = defaultValue;
        ipf_self.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnInputFieldEndEdit(string value)
    {
        Limit();
        OnEndEdit?.Invoke(ipf_self.text);
    }

    protected abstract void Limit();
}
