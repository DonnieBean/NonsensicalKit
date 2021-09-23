using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonsensicalKit.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RenderBox : MonoBehaviour
{

    public Bounds box;

    private void Reset()
    {
        CalculateBox();
    }

    public void CalculateBox()
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
        box = bounds;
    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(RenderBox))]
public class RenderBoxEditor : Editor
{
    private Quaternion crtRotation;

    private RenderBox crtRenderBox;

    public void OnSceneGUI()
    {
        crtRenderBox = target as RenderBox;
        crtRotation = crtRenderBox.transform.rotation;

        Vector3 pos = crtRenderBox.transform.position;
        Vector3 boundsCenter = crtRenderBox.box.center;
        Vector3 boundsSize = crtRenderBox.box.size;

        Handles.color = Color.blue;

        Vector3 p1 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(1, 1, 1)));
        Vector3 p2 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(1, 1, -1)));
        Vector3 p3 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(1, -1, 1)));
        Vector3 p4 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(1, -1, -1)));
        Vector3 p5 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(-1, 1, 1)));
        Vector3 p6 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(-1, 1, -1)));
        Vector3 p7 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(-1, -1, 1)));
        Vector3 p8 = pos + crtRotation * (boundsCenter + Vector3.Scale(boundsSize * 0.5f, new Vector3(-1, -1, -1)));

        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p1, p3);
        Handles.DrawLine(p2, p4);
        Handles.DrawLine(p3, p4);

        Handles.DrawLine(p5, p6);
        Handles.DrawLine(p5, p7);
        Handles.DrawLine(p6, p8);
        Handles.DrawLine(p7, p8);

        Handles.DrawLine(p1, p5);
        Handles.DrawLine(p2, p6);
        Handles.DrawLine(p3, p7);
        Handles.DrawLine(p4, p8);

        if (editBox)
        {
            Vector3 p9 = pos + crtRotation * (boundsCenter + new Vector3(boundsSize.x * 0.5f, 0, 0));
            Vector3 p10 = pos + crtRotation * (boundsCenter + new Vector3(-boundsSize.x * 0.5f, 0, 0));
            Vector3 p11 = pos + crtRotation * (boundsCenter + new Vector3(0, boundsSize.y * 0.5f, 0));
            Vector3 p12 = pos + crtRotation * (boundsCenter + new Vector3(0, -boundsSize.y * 0.5f, 0));
            Vector3 p13 = pos + crtRotation * (boundsCenter + new Vector3(0, 0, boundsSize.z * 0.5f));
            Vector3 p14 = pos + crtRotation * (boundsCenter + new Vector3(0, 0, -boundsSize.z * 0.5f));

            Vector3 p15 = pos + crtRotation * boundsCenter;


            CheckOnePoint(ref crtRenderBox.box, p9, p15, new Vector3(1, 0, 0));
            CheckOnePoint(ref crtRenderBox.box, p10, p15, new Vector3(-1, 0, 0));
            CheckOnePoint(ref crtRenderBox.box, p11, p15, new Vector3(0, 1, 0));
            CheckOnePoint(ref crtRenderBox.box, p12, p15, new Vector3(0, -1, 0));
            CheckOnePoint(ref crtRenderBox.box, p13, p15, new Vector3(0, 0, 1));
            CheckOnePoint(ref crtRenderBox.box, p14, p15, new Vector3(0, 0, -1));
        }
    }

    private void CheckOnePoint(ref Bounds box, Vector3 holdPoint, Vector3 centerPoint, Vector3 dir)
    {
        EditorGUI.BeginChangeCheck();

        Vector3 newHoldPoint = Handles.FreeMoveHandle(holdPoint, Quaternion.identity, HandleUtility.GetHandleSize(holdPoint) * 0.03f, Vector3.one * 0.5f, Handles.DotHandleCap);

        if (EditorGUI.EndChangeCheck())
        {

            Undo.RecordObject(crtRenderBox, "Changed Box");
            var v = VectorHelper.GetFootDrop(newHoldPoint, centerPoint, holdPoint);
            var v2 = (v - holdPoint).magnitude;

            if (Vector3.Angle(v - holdPoint, holdPoint - centerPoint) > 90)
            {
                v2 = -v2;
            }

            box.size += Vector3.Scale(Vector3.Scale(Vector3.one * v2, dir), dir);
            box.center += Vector3.Scale(Vector3.one * v2 * 0.5f, dir);
        }
        EditorGUI.BeginChangeCheck();
    }

    /// <summary>
    /// 不自动面朝相机的Cube
    /// </summary>
    /// <param name="controlID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="size"></param>
    /// <param name="eventType"></param>
    private void CustomHandle(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
        switch (eventType)
        {
            case EventType.Repaint:
                {
                    Vector3 cubeSize = Vector3.one * 0.5f;

                    Graphics.DrawMeshNow(ModelHelper.GetCube(Vector3.zero, cubeSize), StartCapDraw(position, crtRotation, size));

                }
                break;
            case EventType.Layout:
            case EventType.MouseMove:
                HandleUtility.AddControl(controlID, HandleUtility.DistanceToCube(position, rotation, size));
                break;
        }
    }

    private Color realHandleColor { get { return Handles.color * new Color(1, 1, 1, .5f) + (Handles.lighting ? new Color(0, 0, 0, .5f) : new Color(0, 0, 0, 0)); } }


    private Matrix4x4 StartCapDraw(Vector3 position, Quaternion rotation, float size)
    {
        Shader.SetGlobalColor("_HandleColor", realHandleColor);
        Shader.SetGlobalFloat("_HandleSize", size);
        Matrix4x4 mat = Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one);
        Shader.SetGlobalMatrix("_ObjectToWorld", mat);
        HandleUtility.handleMaterial.SetInt("_HandleZTest", (int)Handles.zTest);
        HandleUtility.handleMaterial.SetPass(0);
        return mat;
    }
    bool editBox=false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RenderBox rb = (RenderBox)target;
        
        editBox = GUILayout.Toggle(editBox, "Edit Box");
        
        if (GUILayout.Button("ResetBox"))
        {
            Undo.RecordObject(crtRenderBox, "Reset Box");
            rb.CalculateBox();
        }
    }
}

#endif