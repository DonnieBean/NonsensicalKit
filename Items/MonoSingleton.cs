using UnityEngine;

namespace NonsensicalKit
{
    public abstract class MonoSingleton<T> : NonsensicalMono where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Instance != null)
            {
                return;
            }

            Instance = this as T;
        }
    }
}
