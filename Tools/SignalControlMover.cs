using NonsensicalKit.Utility;
using UnityEngine;

namespace NonsensicalKit
{
    public class SignalControlMover : NonsensicalMono
    {
        [SerializeField] private string signal;
        [SerializeField] private Transform target;
        [SerializeField] private Transform[] pos;
        [SerializeField] private bool isSpeed;

      protected  Tweenner crtTweener;
        protected override void Awake()
        {
            base.Awake();
            
            Subscribe<int, float>(signal, OnMove);
        }

        private void OnMove(int index, float value)
        {
            if (isSpeed)
            {
                float distance = Vector3.Distance(target.position,pos[index].position);
                crtTweener= target.DoMove(pos[index].position,distance/value);
            }
            else
            {
                crtTweener = target.DoMove(pos[index].position, value);
            }
        }
    }

}
