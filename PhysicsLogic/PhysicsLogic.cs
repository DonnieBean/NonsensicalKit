using UnityEngine;

namespace NonsensicalKit.PhysicsLogic
{
    //ʹ��ʱ��Ҫ��ProjectSetting/Physics/ContactPairsMode��ΪEnableKinematicStaticPairs
    public static class PhysicsLogicConst
    {
        /// <summary>
        /// �����߼�����ʹ�õ�Tag
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