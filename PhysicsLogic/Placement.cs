using UnityEngine;

namespace NonsensicalKit.PhysicsLogic
{
    /// <summary>
    /// 安置点：存放物料的对象
    /// </summary>
    public abstract class Placement : NonsensicalMono
    {
       [SerializeField] protected ModelSetting[] modelSettings;

        protected Materials crtMaterial;

        public Materials CrtMaterials { set { crtMaterial = value; SetPositionAndRotation(); } }

        public bool CanSet(string targetName)
        {
            if (crtMaterial)
            {
                return false;
            }
            foreach (var item in modelSettings)
            {
                if (item.physicsLogicName==targetName)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetPositionAndRotation()
        {
            if (crtMaterial == null)
            {
                return;
            }
            crtMaterial.transform.SetParent(transform);
            string crtName = crtMaterial.Name;
            foreach (var item in modelSettings)
            {
                if (item.physicsLogicName == crtName)
                {
                    crtMaterial.transform.localPosition = item.offsetPos;
                    crtMaterial.transform.localEulerAngles = item.offsetRot;
                }
            }
        }
    }
}

