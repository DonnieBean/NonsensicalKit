using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldLimitFloatRange : InputFieldLimitBase
{
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;
    protected override void Awake()
    {
        base.Awake();

        if (minValue>maxValue)
        {
            maxValue = minValue;
        }
        ipf_self.contentType = UnityEngine.UI.InputField.ContentType.DecimalNumber;
    }

    protected override void Limit()
    {
        if (float.TryParse(ipf_self.text,out var v))
        {
            if (v<minValue)
            {
                ipf_self.text = minValue.ToString();
            }
            else if(v>maxValue)
            {
                ipf_self.text = maxValue.ToString();
            }
        }
        else
        {
            ipf_self.text = defaultValue;
        }
    }
}
