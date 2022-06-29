using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBox : NonsensicalMono
{
    [SerializeField] private bool initState = true;

    private Bounds bounds;

    public bool state { get; set; }

    protected override void Awake()
    {
        base.Awake();
        bounds = CalculateBox();
    }

    private void Start()
    {
        ChangeState(initState);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ChangeState(false);
    }

    private void ChangeState(bool newState)
    {
        if (newState != state)
        {
            state = newState;
            if (state)
            {
                Publish<Transform, Bounds>("addDrawBox", transform, bounds);
            }
            else
            {

                Publish<Transform>("removeDrawBox", transform);
            }
        }
    }
    public Bounds CalculateBox()
    {
        Quaternion qn = transform.rotation;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        bool hasBounds = false;

        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

        Renderer[] childRenderers = transform.GetComponentsInChildren<Renderer>();

        foreach (var item in childRenderers)
        {
            if (hasBounds)
            {
                bounds.Encapsulate(item.bounds);
            }
            else
            {
                bounds = item.bounds;
                hasBounds = true;
            }

        }

        transform.rotation = qn;
        bounds.center -= transform.position;
        return bounds;
    }
}
