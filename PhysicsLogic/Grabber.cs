using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.PhysicsLogic
{

    /// <summary>
    /// 抓取者：可吸附物料跟随移动
    /// </summary>
    public abstract class Grabber : NonsensicalMono
    {
        [SerializeField] protected ModelSetting[] modelSettings;   //可以吸附的物料链表

        protected List<Materials> touchMaterials = new List<Materials>();     //当前接触到的所有物料
        protected Materials adsorbMaterials;     //当前吸附中的物料

        protected bool CanAbsorb(string targetName)
        {
            foreach (var item in modelSettings)
            {
                if (item.physicsLogicName == targetName)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == PhysicsLogicConst.Tag)
            {
                if (collision.transform.TryGetComponent(out Materials materials))
                {
                    OnEnter(materials);
                    if (CanAbsorb(materials.Name) && touchMaterials.Contains(materials) == false)
                    {
                        touchMaterials.Add(materials);

                    }
                }
            }
            else
            {
                Debug.Log("撞到了其他物体：" + collision.gameObject.name);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == PhysicsLogicConst.Tag)
            {
                if (collision.gameObject.TryGetComponent(out Materials materials))
                {
                    if (touchMaterials.Contains(materials) == true)
                    {
                        touchMaterials.Remove(materials);
                        OnExit();
                    }
                }
            }
        }

        protected virtual void OnEnter(Materials materials) { }
        protected virtual void OnExit() { }

        /// <summary>
        /// 增加刚体，进行碰撞判断
        /// </summary>
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

        /// <summary>
        ///  删除刚体，不进行碰撞判断
        /// </summary>
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

        public void SetPositionAndRotation()
        {
            if (adsorbMaterials==null)
            {
                return;
            }
            adsorbMaterials.transform.SetParent(transform);
            string crtName = adsorbMaterials.Name;
            foreach (var item in modelSettings )
            {
                if (item.physicsLogicName==crtName)
                {
                    adsorbMaterials.transform.localPosition = item.offsetPos;
                    adsorbMaterials.transform.localEulerAngles = item.offsetRot;
                }
            }
        }
    }
}
