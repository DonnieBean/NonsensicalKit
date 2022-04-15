using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֧��ÿ����һ�����ɶȵ����,��������Ӧ��һһ��Ӧ��ÿ�������
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
                Debug.Log("�ڵ㳤�Ȳ���ȷ");
                this.enabled = false;   //�ر������ֹFixedUpdate����
            }
        }
    }

    private void FixedUpdate()
    {
        //��ȡ����ֵ�����������ű�ͬʱ����ĳЩ������
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
