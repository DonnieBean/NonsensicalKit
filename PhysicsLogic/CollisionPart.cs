using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPart : MonoBehaviour
{

    public Action<GameObject> TriggerEnter;
    public Action<GameObject> TriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other.gameObject);
    }
}
