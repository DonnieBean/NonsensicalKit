using UnityEngine;

namespace NonsensicalKit.PhysicsLogic
{
    public class SignalMaterials : Materials
    {
        /// <summary>
        /// 是否可以被放置
        /// </summary>
        public override bool CanSetModel
        {
            get
            {
                //if (crtPlacement == null || crtPlacement.CanSetModel == false)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}

                return false;
            }
        }

        public override bool CanAdsorb
        {
            get
            {
                //return !isAdsorbing;

                return false;
            }
        }
        public void Adsorbing(Transform parent)
        {
            //transform.SetParent(parent);
            //transform.localPosition = offsetPos;
            //transform.localEulerAngles = offsetRot;
            //if (crtPlacement != null)
            //{
            //    crtPlacement.GetModel();
            //}
            //isAdsorbing = true;
        }

        public void SetModel()
        {
            //isAdsorbing = false;

            //crtPlacement.SetModel(gameObject);
        }
    }

}

