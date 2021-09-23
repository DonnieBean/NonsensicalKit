using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkPlacementTest : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private ForkPlacement testTarget;

    private void Start()
    {
        var v = Instantiate(prefab);

        if (v.TryGetComponent<ForkMaterials>(out var fm))
        {
            fm.Init(testTarget);
        }
    }
}
