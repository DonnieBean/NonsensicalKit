using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 支持每个轴一个自由度的情况,所有轴完全自由指定
/// </summary>
public class ArticulationBodyFreedomRobot : ArticulationBodyRobotBase
{
    protected override void Awake()
    {
        base.Awake(); 
        startData = new float[joints.Length];
        var indexs = new List<int>();
        for (int i = 0; i < joints.Length; i++)
        {
            var item = joints[i];
            item.joint.GetDofStartIndices(indexs);
            item.trueIndex = indexs[item.joint.index];
            item.buffer = new List<float>();
            item.joint.GetDriveTargets(item.buffer);
            startData[i] = item.buffer[item.trueIndex];
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

    public override void ResetRobot()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            var item = joints[i];
            item.joint.GetDriveTargets(item.buffer);
            item.buffer[item.trueIndex] = startData[i];
            item.joint.SetDriveTargets(item.buffer);
        }
    }
}
