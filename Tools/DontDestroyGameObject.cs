using UnityEngine;

namespace NonsensicalKit.Tools
{
    public class DontDestroyGameObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}