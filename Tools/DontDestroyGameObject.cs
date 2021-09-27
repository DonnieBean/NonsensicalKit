using UnityEngine;

namespace NonsensicalKit
{
    public class DontDestroyGameObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}