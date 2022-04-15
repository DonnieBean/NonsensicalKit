using NonsensicalKit.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 自动生成不平整的地面
    /// </summary>
    public class AutoPlane : MonoBehaviour
    {
        [SerializeField] private float size=1;
        [SerializeField] private int count=10;

        private  void Awake()
        {
            gameObject.AddComponent<MeshFilter>().mesh = ModelHelper.CreateUnevenPlane(Vector3.zero, size,count);
            gameObject.AddComponent<MeshRenderer>().material = ModelHelper.GetDiffuseMaterial();
            gameObject.AddComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;

        }
    }
}
