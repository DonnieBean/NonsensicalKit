namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 管理类应继承此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NonsensicalManagerBase<T> : NonsensicalMono where T : class
    {
        public static T Instance;

        protected override void Awake()
        {
            base.Awake();

            Instance = this as T;

            Subscribe((uint)NonsensicalManagerEnum.InitStart, InitStart);
            Subscribe((uint)NonsensicalManagerEnum.LateInitStart, LateInitStart);
            Subscribe((uint)NonsensicalManagerEnum.FinalInitStart, FinalInitStart);
        }

        protected virtual void Start()
        {
            Publish((uint)NonsensicalManagerEnum.ManagerSubscribe);
        }

        protected void InitComplete()
        {
            Publish((uint)NonsensicalManagerEnum.InitComlete);
        }

        protected void LateInitComplete()
        {
            Publish((uint)NonsensicalManagerEnum.LateInitComlete);
        }
        protected void FinalInitComplete()
        {
            Publish((uint)NonsensicalManagerEnum.FinalInitComlete);
        }


        protected abstract void InitStart();
        protected abstract void LateInitStart();
        protected abstract void FinalInitStart();
    }
}