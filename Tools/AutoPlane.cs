using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    public class AutoPlane : MonoBehaviour
    {
        public float size=1;
        public int count=10;

        private  void Awake()
        {
            gameObject.AddComponent<MeshFilter>().mesh = ModelHelper.GetUnevenPlane(Vector3.zero, size,count);
            gameObject.AddComponent<MeshRenderer>().material = ModelHelper.GetDiffuseMaterial();

        }
    }
}
