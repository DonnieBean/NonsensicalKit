using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public JointState state;

    public List<float> buffer { get; set; }
    public int trueIndex { get; set; }
}

public abstract class ArticulationBodyRobotBase : NonsensicalMono
{

    [SerializeField] protected ArticulationJoint[] joints;

    protected float[] startData;


    public void SetState(int index, JointState state)
    {
        joints[index].state = state;
    }

    public JointState GetState(int index)
    {
        return joints[index].state;
    }

    public abstract void ResetRobot();
}
