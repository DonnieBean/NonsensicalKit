using NonsensicalKit.Utility;
#if USE_FINALIK
using RootMotion.FinalIK;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.Joint
{
#if USE_FINALIK
    [System.Serializable]
    public struct ValueLimit
    {
        public float Min;
        public float Max;
    }

    [System.Serializable]
    public struct SpaceLimit
    {
        public float XMin;
        public float XMax;
        public float YMin;
        public float YMax;
        public float ZMin;
        public float ZMax;

        public bool Clamp(Vector3 value, out Vector3 newValue)
        {
            bool flag = false;

            if (value.x < XMin)
            {
                flag = true;
                value.x = XMin;
            }
            else if (value.x > XMax)
            {
                flag = true;
                value.x = XMax;
            }

            if (value.y < YMin)
            {
                flag = true;
                value.y = YMin;
            }
            else if (value.y > YMax)
            {
                flag = true;
                value.y = YMax;
            }

            if (value.z < ZMin)
            {
                flag = true;
                value.z = ZMin;
            }
            else if (value.z < ZMax)
            {
                flag = true;
                value.z = ZMax;
            }

            newValue = value;
            return flag;
        }

        public Vector3 Clamp(Vector3 value)
        {
            if (value.x < XMin)
            {
                value.x = XMin;
            }
            else if (value.x > XMax)
            {
                value.x = XMax;
            }

            if (value.y < YMin)
            {
                value.y = YMin;
            }
            else if (value.y > YMax)
            {
                value.y = YMax;
            }

            if (value.z < ZMin)
            {
                value.z = ZMin;
            }
            else if (value.z < ZMax)
            {
                value.z = ZMax;
            }

            return value;
        }
    }

    public class JointControllerWithCCDIK : JointController
    {
        [SerializeField] private CCDIK ccdIK;

        [SerializeField] private ValueLimit[] valueLimits;
        [SerializeField] private SpaceLimit spaceLimit;

        [SerializeField] private Transform spaceCenter;
        [SerializeField] private Transform target;
        [SerializeField] private Transform targetPos;
        [SerializeField] private float spaceConversionRate;

        public void ChangeMode(bool isBaseMode)
        {
            if (isBaseMode)
            {
                ccdIK.enabled = true;
                target.position = targetPos.position;
            }
            else
            {
                ccdIK.enabled = false;
                targetPos.position = target.position;
            }
        }

        public void ChangeSpaceMode()
        {
            ccdIK.enabled = true;
            target.position = targetPos.position;
        }

        public void ChangeJointMode()
        {
            ccdIK.enabled = false;
            targetPos.position = target.position;
        }

        public void ChangeSpaceValue(Vector3 offset)
        {
            Vector3 realOffset = spaceCenter.forward * offset.x + -spaceCenter.right * offset.y + spaceCenter.up * offset.z;

            target.position += realOffset;

        }

        public void ChangeSpaceValue(Vector3 offset, float time)
        {
            Vector3 realOffset = spaceCenter.forward * offset.x + -spaceCenter.right * offset.y + spaceCenter.up * offset.z;

            target.DoMove(target.position + realOffset, time);
        }
        public void SetSpaceValue(Vector3 targetValue, float time)
        {
            Vector3 realValue = spaceCenter.forward * targetValue.x + -spaceCenter.right * targetValue.y + spaceCenter.up * targetValue.z;

            target.DoMove(realValue, time);
        }

        public void ChangeJointValue(float[] offset)
        {
            float[] now = GetJointsValue();

            float[] target = NumHelper.ArrayPlus(now, offset);

            ChangeState(new ActionData(target, 0));
        }

        public void ChangeJointValue(float[] offset, float time)
        {
            float[] now = GetJointsValue();

            float[] target = NumHelper.ArrayPlus(now, offset);

            ChangeState(new ActionData(target, time));
        }
        public void SetJointValue(float[] targetValue, float time)
        {
            Debug.Log(StringHelper.GetSetString(targetValue));
            ChangeState(new ActionData(targetValue, time));
        }

        public float[] GetJointsValue()
        {
            float[] values = new float[joints.Length];

            for (int i = 0; i < joints.Length; i++)
            {
                float crtValue = 0;
                Vector3 gap = Vector3.zero;

                if (joints[i].axisType == AxisType.position)
                {
                    gap = (joints[i].jointsNode.localPosition - joints[i].zeroState) / joints[i].conversionRate;


                }
                else if (joints[i].axisType == AxisType.rotation)
                {

                    //gap = Vector3.one* Quaternion.Angle(Quaternion.Euler(joints[i].jointsNode.localEulerAngles), Quaternion.Euler(joints[i].zeroState));

                    gap = (joints[i].jointsNode.localEulerAngles - joints[i].zeroState) / joints[i].conversionRate;
                }

                switch (joints[i].dirType)
                {
                    case DirType.X:
                        crtValue = gap.x;
                        break;
                    case DirType.Y:
                        crtValue = gap.y;
                        break;
                    case DirType.Z:
                        crtValue = gap.z;
                        break;
                    case DirType.X_:
                        crtValue = -gap.x;
                        break;
                    case DirType.Y_:
                        crtValue = -gap.y;
                        break;
                    case DirType.Z_:
                        crtValue = -gap.z;
                        break;
                    default:
                        crtValue = 0;
                        break;
                }


                if (joints[i].axisType == AxisType.rotation && crtValue < -180)
                {
                    crtValue += 360;
                }

                values[i] = crtValue;
            }
            return values;
        }

        public Vector3 GetSpaceValue()
        {
            Vector3 offset = target.position - spaceCenter.position;

            float xValue = VectorHelper.GetAxisValue(target.position, spaceCenter.position, spaceCenter.position + spaceCenter.forward);
            float yValue = VectorHelper.GetAxisValue(target.position, spaceCenter.position, spaceCenter.position - spaceCenter.right);
            float zValue = VectorHelper.GetAxisValue(target.position, spaceCenter.position, spaceCenter.position + spaceCenter.up);

            return new Vector3(xValue, yValue, zValue) / spaceConversionRate;
        }


    }
#endif

}
