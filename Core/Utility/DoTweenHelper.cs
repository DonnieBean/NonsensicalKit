using UnityEngine;

namespace NonsensicalKit.Utility
{
    /// <summary>
    /// 仿DOTween，作为简单需求时的替代品
    /// </summary>
    public static class DoTweenHelper
    {
        public static Tweenner DoFade(this CanvasGroup canvasGroup, float endValue, float value)
        {
            CanvasGroupTweener newTweener = new CanvasGroupTweener(canvasGroup, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }

        public static Tweenner DoMove(this Transform _transform, Vector3 endValue, float value)
        {
            TransformMoveTweener newTweener = new TransformMoveTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }

        public static Tweenner DoRotate(this Transform _transform, Vector3 endValue, float value)
        {
            TransformRotateTweener newTweener = new TransformRotateTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }

        public static Tweenner DoQuaternionRotate(this Transform _transform, Quaternion endValue, float value)
        {
            TransformQuaternionRotateTweener newTweener = new TransformQuaternionRotateTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }

        public static Tweenner DoLocalMove(this Transform _transform, Vector3 endValue, float value)
        {
            TransformLocalMoveTweener newTweener = new TransformLocalMoveTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }


        public static Tweenner DoLocalMoveX(this Transform _transform, float endValue, float value)
        {
            TransformLocalMoveXTweener newTweener = new TransformLocalMoveXTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }

        public static Tweenner DoLocalRotate(this Transform _transform, Vector3 endValue, float value)
        {
            TransformLocalRotateTweener newTweener = new TransformLocalRotateTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }
        public static Tweenner DoLocalRotate(this Transform _transform, Quaternion endValue, float value)
        {
            TransformQuaternionLocalRotateTweener newTweener = new TransformQuaternionLocalRotateTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }
        public static Tweenner DoLocalScale(this Transform _transform, Vector3 endValue, float value)
        {
            TransformLocalScaleTweener newTweener = new TransformLocalScaleTweener(_transform, endValue, value);

            NonsensicalUnityInstance.Instance.tweenners.Add(newTweener);

            return newTweener;
        }

     
    }

    public abstract class Tweenner
    {
        /// <summary>
        /// 默认是运动的总时间，依据速度运动时是速度值
        /// </summary>
        private readonly float value;
        /// <summary>
        /// 总运动量，用于以速度运动时的进度依据
        /// </summary>
        protected  float totalValue;
        private float delay;

        /// <summary>
        /// 累积时间
        /// </summary>
        protected float accumulateTime;

        public bool NeedAbort;
        private bool isPause;
        public bool speedBase;

        public delegate void OnCompleteHander();
        public OnCompleteHander OnCompleteEvent;

        protected Tweenner(float _value)
        {
            NeedAbort = DoSpecificBySchedule(0);
            value = _value;
            accumulateTime = 0;
            delay = 0;
            NeedAbort = false;
            isPause = false;
        }


        /// <summary>
        /// 由NonsensicalUnityInstance每帧调用的方法
        /// </summary>
        /// <param name="_deltaTime"></param>
        /// <returns></returns>
        public bool DoIt(float _deltaTime)
        {
            if (isPause)
            {
                return false;
            }

            float schedule;
            accumulateTime += _deltaTime;
            if (speedBase)
            {
                if (value<=0|| totalValue == 0)
                {
                    schedule = 1;
                }
                else
                {
                    schedule = (accumulateTime - delay) * value / totalValue;
                }
            }
            else
            {
                if (value<=0)
                {
                    schedule = 1;
                }
                else
                {
                    schedule = (accumulateTime - delay) / value;
                }
            }

            if (schedule >= 1)
            {
                NeedAbort = DoSpecificBySchedule(1);
                OnCompleteEvent?.Invoke();
                return true;
            }
            else
            {
                NeedAbort = DoSpecificBySchedule(schedule);
                return false;
            }
        }

        public void Pause()
        {
            isPause = true;
        }

        public void Resume()
        {
            isPause = false;
        }

        public void Abort()
        {
            NeedAbort = true;
        }

        /// <summary>
        /// 某一次调用后以进度为依据执行的具体行为
        /// </summary>
        /// <param name="_schedule">区间为0到1的进度值</param>
        /// <returns></returns>
        public abstract bool DoSpecificBySchedule(float _schedule);

        public Tweenner SetDelay(float _time)
        {
            delay = _time;
            return this;
        }

        /// <summary>
        /// 将传入的第二个参数作为速度使用，其中位移的速度单位是m/s，旋转的速度单位是°/s
        /// </summary>
        /// <returns></returns>
        public Tweenner SetSpeedBased()
        {
            speedBase = true;
            return this;
        }

        public Tweenner OnComplete(OnCompleteHander _func)
        {
            OnCompleteEvent += _func;
            return this;
        }

    }

    public class CanvasGroupTweener : Tweenner
    {
        private readonly CanvasGroup canvasGroup;
        private readonly float startValue;
        private readonly float endValue;

        public CanvasGroupTweener(CanvasGroup _canvasGroup, float _endValue, float _value) : base(_value)
        {
            canvasGroup = _canvasGroup;
            startValue = _canvasGroup.alpha;
            endValue = _endValue;
            totalValue = Mathf.Abs( _endValue - startValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (canvasGroup == null)
            {
                return true;
            }
            canvasGroup.alpha = startValue + (endValue - startValue) * schedule;

            return false;
        }
    }

    public class TransformRotateTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Vector3 startValue;
        private readonly Vector3 endValue;
        public TransformRotateTweener(Transform _transform, Vector3 _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.eulerAngles;
            endValue = _endValue.AngleNear(startValue);
            totalValue =Quaternion.Angle( Quaternion.Euler(startValue ), Quaternion.Euler(_endValue));
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.eulerAngles = Vector3.Lerp(startValue, endValue, schedule);

            return false;
        }
    }

    public class TransformQuaternionRotateTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Quaternion startValue;
        private readonly Quaternion endValue;
        public TransformQuaternionRotateTweener(Transform _transform, Quaternion _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.rotation;
            endValue = _endValue;
            totalValue = Quaternion.Angle(startValue, _endValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.rotation = Quaternion.Lerp(startValue, endValue, schedule);

            return false;
        }
    }

    public class TransformQuaternionLocalRotateTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Quaternion startValue;
        private readonly Quaternion endValue;
        public TransformQuaternionLocalRotateTweener(Transform _transform, Quaternion _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.localRotation;
            endValue = _endValue;
            totalValue = Quaternion.Angle(startValue, _endValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.localRotation = Quaternion.Lerp(startValue, endValue, schedule);

            return false;
        }
    }

    public class TransformMoveTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Vector3 startValue;
        private readonly Vector3 endValue;
        public TransformMoveTweener(Transform _transform, Vector3 _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.position;
            endValue = _endValue;
            totalValue = Vector3.Distance(startValue, _endValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.position = Vector3.Lerp(startValue, endValue, schedule);

            return false;
        }
    }

    public class TransformLocalMoveTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Vector3 startValue;
        private readonly Vector3 endValue;
        public TransformLocalMoveTweener(Transform _transform, Vector3 _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.localPosition;
            endValue = _endValue;
            totalValue = Vector3.Distance(startValue, _endValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.localPosition = Vector3.Lerp(startValue, endValue, schedule);

            return false;
        }
    }

    public class TransformLocalRotateTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Vector3 startValue;
        private readonly Vector3 endValue;
        public TransformLocalRotateTweener(Transform _transform, Vector3 _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.localEulerAngles;
            endValue = _endValue.AngleNear(startValue);
            totalValue = Quaternion.Angle(Quaternion.Euler(startValue), Quaternion.Euler(_endValue));
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.localEulerAngles = Vector3.Lerp(startValue, endValue, schedule);
            return false;
        }
    }
    public class TransformLocalScaleTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly Vector3 startValue;
        private readonly Vector3 endValue;
        public TransformLocalScaleTweener(Transform _transform, Vector3 _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.localScale;
            endValue = _endValue;
            totalValue = Vector3.Distance(startValue, _endValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            transform.localScale = Vector3.Lerp(startValue, endValue, schedule);
            return false;
        }
    }

    public class TransformLocalMoveXTweener : Tweenner
    {
        private readonly Transform transform;
        private readonly float startValue;
        private readonly float endValue;
        public TransformLocalMoveXTweener(Transform _transform, float _endValue, float _value) : base(_value)
        {
            transform = _transform;
            startValue = _transform.localPosition.x;
            endValue = _endValue;
            totalValue =Mathf.Abs( _endValue-startValue);
        }

        public override bool DoSpecificBySchedule(float schedule)
        {
            if (transform == null)
            {
                return true;
            }
            Vector3 temp = transform.localPosition;
            temp.x = startValue + (endValue - startValue) * schedule;
            transform.localPosition = temp;
            return false;
        }
    }

}

