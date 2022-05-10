using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    public class AutoBox : MonoBehaviour
    {
        [SerializeField] private Vector3 autoBoxSize;

        private void Awake()
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
            primitive.SetActive( false);
            Material diffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
            DestroyImmediate(primitive);
            gameObject.AddComponent<MeshFilter>().mesh = ModelHelper.CreateCube(autoBoxSize.x, autoBoxSize.y, autoBoxSize.z);
            gameObject.AddComponent<MeshRenderer>().material = diffuse;
        }
    }
}
