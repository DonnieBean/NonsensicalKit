using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkGrabberTest : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private ForkGrabber testTarget;

    private void Start()
    {
        var v = Instantiate(prefab);

        if (v.TryGetComponent<ForkMaterials>(out var fm))
        {
            testTarget.SetForkMaterials(fm);
        }
    }
}
