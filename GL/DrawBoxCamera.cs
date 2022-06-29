using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoxCamera : NonsensicalMono
{
    [SerializeField] private Color color = new Color(0, 1, 1, 0.1f);

    [SerializeField] private Material rectMat = null;//这里使用Sprite下的defaultshader的材质即可

    [SerializeField] private bool initState = true;

    private Dictionary<Transform, Bounds> keyValuePairs = new Dictionary<Transform, Bounds>();

    public bool state { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        state = initState;
        Subscribe<Transform, Bounds>("addDrawBox", OnAddBox);
        Subscribe<Transform>("removeDrawBox", OnRemoveBox);
    }

    private void OnAddBox(Transform tsf, Bounds bounds)
    {
        if (keyValuePairs.ContainsKey(tsf) == false)
        {
            keyValuePairs.Add(tsf, bounds);
        }
    }

    private void OnRemoveBox(Transform tsf)
    {
        if (keyValuePairs.ContainsKey(tsf))
        {
            keyValuePairs.Remove(tsf);
        }
    }

    void OnPostRender()
    {
        if (state)
        {
            GL.PushMatrix();
            rectMat.SetPass(0);//为渲染激活给定的pass。

            GL.Begin(GL.QUADS);
            GL.Color(color);
            foreach (var item in keyValuePairs)
            {
                Quaternion crtRotation = item.Key.rotation;
                Vector3 pos = item.Key.position;
                Bounds box = item.Value;
                Vector3 boundsCenter = box.center;
                Vector3 boundsSize = box.size * 0.55f;    //向外扩张十分之一

                Vector3 p1 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(1, 1, 1)));
                Vector3 p2 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(1, 1, -1)));
                Vector3 p3 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(1, -1, 1)));
                Vector3 p4 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(1, -1, -1)));
                Vector3 p5 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(-1, 1, 1)));
                Vector3 p6 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(-1, 1, -1)));
                Vector3 p7 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(-1, -1, 1)));
                Vector3 p8 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize, new Vector3(-1, -1, -1)));

                GL.Vertex(p1);
                GL.Vertex(p2);
                GL.Vertex(p4);
                GL.Vertex(p3);

                GL.Vertex(p5);
                GL.Vertex(p6);
                GL.Vertex(p8);
                GL.Vertex(p7);

                GL.Vertex(p1);
                GL.Vertex(p2);
                GL.Vertex(p6);
                GL.Vertex(p5);

                GL.Vertex(p3);
                GL.Vertex(p4);
                GL.Vertex(p8);
                GL.Vertex(p7);

                GL.Vertex(p1);
                GL.Vertex(p3);
                GL.Vertex(p7);
                GL.Vertex(p5);

                GL.Vertex(p2);
                GL.Vertex(p4);
                GL.Vertex(p8);
                GL.Vertex(p6);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
