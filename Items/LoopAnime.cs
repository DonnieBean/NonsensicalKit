using NonsensicalKit;
using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送带上物体动画
/// </summary>
public class LoopAnime : NonsensicalMono
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float interval;
    [SerializeField] private float time;
    [SerializeField] private string stateSignal;
    private GameObjectPool gop;

    private bool crtState = true;

    private List<GameObject> gos = new List<GameObject>();

    protected override  void Awake()
    {
        base.Awake();
        gop = new GameObjectPool(prefab, (go) =>{ go.SetActive(false); }, (go) => { go.SetActive(true); },(go)=> { gos.Add(go); go.transform.SetParent(transform); });

        StartCoroutine(Run());
        Subscribe<bool>(stateSignal,OnStateChanged);
    }

    private void OnStateChanged(bool state)
    {
        if (state!= crtState)
        {
            crtState = state;
            if (state==true)
            {

                StartCoroutine(Run());
            }
            else
            {
                StopAllCoroutines();
                foreach (var item in gos)
                {
                    gop.Store(item);
                }
                gos.Clear();
            }
        }
    }

    private IEnumerator Run()
    {
        float timer=0;
        while (true)
        {
            timer -= Time.deltaTime;
            if (timer<0)
            {
                timer = interval;
                GameObject newGO = gop.New();
                newGO.transform.position = startPoint.position;
                newGO.transform.DoMove(endPoint.position,time).OnCompleteEvent+=()=> { gop.Store(newGO); };
            }
            yield return null;
        }
    }
}
