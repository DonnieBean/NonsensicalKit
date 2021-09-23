using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.PhysicsLogic
{

    /// <summary>
    /// ץȡ�ߣ����������ϸ����ƶ�
    /// </summary>
    public abstract class Grabber : NonsensicalMono
    {
        [SerializeField] protected ModelSetting[] modelSettings;   //������������������

        protected List<Materials> touchMaterials = new List<Materials>();     //��ǰ�Ӵ�������������
        protected Materials adsorbMaterials;     //��ǰ�����е�����

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
                Debug.Log("ײ�����������壺" + collision.gameObject.name);
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
        /// ���Ӹ��壬������ײ�ж�
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
        ///  ɾ�����壬��������ײ�ж�
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
