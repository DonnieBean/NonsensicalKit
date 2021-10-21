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


       protected void ResetPos()
        {
            crtTweener?.Abort();
            target.DoMove(pos[0].position,0.1f);
        }



        private void OnMove(int index, float value)
        {
            if (crtTweener!=null)
            {
                crtTweener.Abort();
            }
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
