using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldLimitFloat : InputFieldLimitBase
{
    protected override void Awake()
    {
        base.Awake();

        ipf_self.contentType = UnityEngine.UI.InputField.ContentType.DecimalNumber;
    }

    protected override void Limit()
    {
        if (!float.TryParse(ipf_self.text, out var v))
        {
            ipf_self.text = defaultValue;
        }
    }
}
