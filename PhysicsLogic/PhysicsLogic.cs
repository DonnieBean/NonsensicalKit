using UnityEngine;

namespace NonsensicalKit.PhysicsLogic
{
    //使用时需要将ProjectSetting/Physics/ContactPairsMode改为EnableKinematicStaticPairs
    public static class PhysicsLogicConst
    {
        /// <summary>
        /// 物理逻辑对象使用的Tag
        /// </summary>
        public  const string Tag = "PhysicsLogicObject";
    }

    [System.Serializable]
    public struct ModelSetting
    {
        [CustomLabel("Name")] public string physicsLogicName;
        public Vector3 offsetPos;
        public Vector3 offsetRot;
    }

    public enum PhysicsLogicEnum
    {

    }
}