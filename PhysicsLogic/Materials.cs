using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物料,主要的被操作对象
/// </summary>
namespace NonsensicalKit.PhysicsLogic
{
    public abstract class Materials : NonsensicalMono
    {
        [SerializeField] protected string physicsLogicName;
     
        public string Name { get { return physicsLogicName; } }

        /// <summary>
        /// 是否可以被放置
        /// </summary>
        public abstract bool CanSetModel { get; }
        /// <summary>
        /// 是否可以被吸附
        /// </summary>
        /// <returns></returns>
        public abstract bool CanAdsorb { get; }
        protected Placement crtPlacement;
        protected Grabber crtGrabber;


        protected  List<Placement> touchPlacements = new List<Placement>();     //当前接触到的所有物料


        public Grabber CrtGrabber { set { crtGrabber = value; OnGrab(); } }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == PhysicsLogicConst.Tag)
            {
                if (collision.gameObject.TryGetComponent(out Placement placement))
                {
                    OnEnter(placement);
                    if (placement.CanSet(physicsLogicName) && !touchPlacements.Contains(placement))
                    {
                        touchPlacements.Add(placement);
                    }
                }
            }
            else
            {
                Debug.Log("撞到了其他物体" + collision.transform.name);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == PhysicsLogicConst.Tag)
            {
                if (crtPlacement != null)
                {
                    if (collision.gameObject.TryGetComponent(out Placement placement))
                    {
                        if (touchPlacements.Contains(placement))
                        {
                            touchPlacements.Remove(placement);
                            OnExit();
                        }
                    }
                }
            }
        }

        protected virtual void OnEnter(Placement placement) { }

        protected virtual void OnExit() { }
        protected virtual void OnGrab() { }

        public void Activation()
        {
            if (GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }

            GetComponent<Rigidbody>().isKinematic = true;

            MeshCollider[] meshColliders = transform.GetComponentsInChildren<MeshCollider>();

            foreach (var item in meshColliders)
            {
                item.convex = true;
            }
        }

        public void Dormancy()
        {
            if (GetComponent<Rigidbody>() != null)
            {
                Destroy(GetComponent<Rigidbody>());
            }

            MeshCollider[] meshColliders = transform.GetComponentsInChildren<MeshCollider>();

            foreach (var item in meshColliders)
            {
                item.convex = false;
            }
        }
    }
}
