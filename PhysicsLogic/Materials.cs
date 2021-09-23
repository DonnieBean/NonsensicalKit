using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����,��Ҫ�ı���������
/// </summary>
namespace NonsensicalKit.PhysicsLogic
{
    public abstract class Materials : NonsensicalMono
    {
        [SerializeField] protected string physicsLogicName;
     
        public string Name { get { return physicsLogicName; } }

        /// <summary>
        /// �Ƿ���Ա�����
        /// </summary>
        public abstract bool CanSetModel { get; }
        /// <summary>
        /// �Ƿ���Ա�����
        /// </summary>
        /// <returns></returns>
        public abstract bool CanAdsorb { get; }
        protected Placement crtPlacement;
        protected Grabber crtGrabber;


        protected  List<Placement> touchPlacements = new List<Placement>();     //��ǰ�Ӵ�������������


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
                Debug.Log("ײ������������" + collision.transform.name);
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
