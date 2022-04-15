using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public enum JointState
    {
        Fixed = 0,
        Positive = 1,
        Negative = -1,
    }

    [System.Serializable]
    public class ArticulationJoint
    {
        public ArticulationBody joint;
        public float speed = 1;
        public List<float> buffer { get; set; }
        public int trueIndex { get; set; }
        public JointState state;
    }

    /// <summary>
    /// 支持每个轴一个自由度的情况
    /// </summary>
    public class ArticulationBodyRobot : MonoBehaviour
    {
        [SerializeField] private ArticulationJoint[] joints;

        private void Awake()
        {
            var indexs = new List<int>();
            foreach (var item in joints)
            {
                item.joint.GetDofStartIndices(indexs);
                item.trueIndex = indexs[item.joint.index];
                item.buffer = new List<float>();
            }
        }

        private void FixedUpdate()
        {
            foreach (var item in joints)
            {
                if (item.state != JointState.Fixed)
                {
                    float drivePostion = item.joint.jointPosition[0];
                    float targetPosition = drivePostion + (float)item.state * Time.fixedDeltaTime * item.speed;

                    item.joint.GetDriveTargets(item.buffer);
                    item.buffer[item.trueIndex] = targetPosition;
                    item.joint.SetDriveTargets(item.buffer);
                }
            }
        }

        public void SetState(int index, JointState state)
        {
            joints[index].state = state;
        }
        public JointState GetState(int index)
        {
            return joints[index].state;
        }
    }

}