using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldLimitFixedValue : InputFieldLimitBase
{
    [SerializeField] private List<string> fixedValues;
    protected override void Limit()
    {
        if (!fixedValues.Contains(ipf_self.text))
        {
            ipf_self.text = defaultValue;
        }
    }
}
