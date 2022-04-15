using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 支持每个轴一个自由度的情况,且所有轴应当一一对应在每个活动轴上
/// </summary>
public class ArticulationBodyCompleteRobot : ArticulationBodyRobotBase
{
    private List<float> buffer;

    protected override void Awake()
    {
        base.Awake();
        if (joints.Length != 0)
        {
            joints[0].joint.GetDriveTargets(buffer);
            if (buffer.Count == joints.Length)
            {
                startData = buffer.ToArray();
            }
            else
            {
                Debug.Log("节点长度不正确");
                this.enabled = false;   //关闭组件防止FixedUpdate运行
            }
        }
    }

    private void FixedUpdate()
    {
        //获取最新值，兼容其他脚本同时控制某些轴的情况
        joints[0].joint.GetDriveTargets(buffer);
        for (int i = 0; i < joints.Length; i++)
        {
            buffer[i] += (float)joints[i].state * Time.fixedDeltaTime * joints[i].speed;
        }
        joints[0].joint.SetDriveTargets(buffer);
    }

    public override void ResetRobot()
    {
        joints[0].joint.SetDriveTargets(new List<float>(startData));
    }
}
