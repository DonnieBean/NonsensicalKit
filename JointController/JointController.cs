using NonsensicalKit;
using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.Joint
{
    public enum AxisType
    {
        noUse,
        rotation,
        position,
    }

    public enum DirType
    {
        X,
        Y,
        Z,
        X_,
        Y_,
        Z_,
    }

    [System.Serializable]
    public class JointSetting
    {
        /// <summary>
        /// 关节节点
        /// </summary>
        public Transform jointsNode;
        /// <summary>
        /// 需要改变的轴
        /// </summary>
        public AxisType axisType;
        /// <summary>
        /// 需要改变的方向
        /// </summary>
        public DirType dirType;
        /// <summary>
        /// 正常姿态时的欧拉角/轴坐标
        /// </summary>
        public Vector3 zeroState;
        /// <summary>
        /// 正常姿态的初始值
        /// </summary>
        public float InitialValue;
        /// <summary>
        /// 转换率（一单位改变需要改变多少角度/位移）
        /// </summary>
        public float conversionRate = 1;
    }

    public class ActionData
    {
        public ActionData()
        {

        }

        public ActionData(float[] values, float time = 0)
        {
            Values = values;
            Time = time;
        }

        /// <summary>
        /// 每个节点的数值
        /// </summary>
        public float[] Values { get; set; }
        /// <summary>
        /// 到达目标关节需要多久
        /// </summary>
        public float Time { get; set; }

        public override string ToString()
        {
            return $"{StringHelper.GetSetString(Values)},time:{Time}";
        }

        public int Length
        {
            get
            {
                return Values.Length;
            }
        }

    }

    /// <summary>
    /// 使用数值控制模型节点位移或者旋转
    /// </summary>
    public class JointController : NonsensicalMono
    {
        public bool IsPause { get; set; }

        public JointSetting[] joints;

        
        private float listTimer;    //贯穿链表运动的计时器，用于校准时间
        private float listTime;

        private bool isList;

        protected virtual void Update()
        {
            if (!IsPause)
            {
                listTimer += Time.deltaTime;
            }
        }

        public void ChangeStates(IEnumerable<ActionData> jds)
        {
            isList = true;
            listTimer = 0;
            listTime = 0;
            StopAllCoroutines();
            StartCoroutine(ChangeStatesCoroutine(jds));
        }

        public void ChangeState(ActionData jd)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeStateCoroutine(jd));
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
        /// <summary>
        /// 用于编辑器环境下的零点重置
        /// </summary>
        public void ResetZeroState()
        {
#if UNITY_EDITOR
            foreach (var item in joints)
            {
                if (item.jointsNode != null)
                {
                    if (item.axisType == AxisType.position)
                    {
                        item.zeroState = item.jointsNode.localPosition;
                    }
                    else if (item.axisType == AxisType.rotation)
                    {
                        item.zeroState = item.jointsNode.localEulerAngles;
                    }

                }
            }
#endif
        }

        private IEnumerator ChangeStatesCoroutine(IEnumerable<ActionData> jds)
        {
            foreach (var item in jds)
            {
                yield return ChangeStateCoroutine(item);
            }
        }

        private IEnumerator ChangeStateCoroutine(ActionData jd)
        {
            float time = jd.Time;
            if (isList)
            {
                listTime += jd.Time;

                time = listTime - listTimer;
            }
            int min = joints.Length > jd.Length ? joints.Length : jd.Length;

            for (int i = 0; i < min - 1; i++)
            {
                StartCoroutine(ChangeJoint(i, jd.Values[i], time));
            }

            yield return ChangeJoint(min - 1, jd.Values[min - 1], time);
        }

        private IEnumerator ChangeJoint(int index, float targetValue, float time)
        {
            JointSetting crtJoint = joints[index];

            float offset = (targetValue - crtJoint.InitialValue) * crtJoint.conversionRate;

            Vector3 v3Offset = Vector3.zero;
            switch (crtJoint.dirType)
            {
                case DirType.X:
                    v3Offset = new Vector3(offset, 0, 0);
                    break;
                case DirType.Y:
                    v3Offset = new Vector3(0, offset, 0);
                    break;
                case DirType.Z:
                    v3Offset = new Vector3(0, 0, offset);
                    break;
                case DirType.X_:
                    v3Offset = new Vector3(-offset, 0, 0);
                    break;
                case DirType.Y_:
                    v3Offset = new Vector3(0, -offset, 0);
                    break;
                case DirType.Z_:
                    v3Offset = new Vector3(0, 0, -offset);
                    break;
            }
            Vector3 targetV3 = crtJoint.zeroState + v3Offset;
            switch (crtJoint.axisType)
            {
                case AxisType.rotation:
                    yield return DoRotate(crtJoint.jointsNode, targetV3, time);
                    break;
                case AxisType.position:
                    yield return DoMove(crtJoint.jointsNode, targetV3, time);
                    break;
            }
        }
        
        private IEnumerator DoRotate(Transform targetTsf, Vector3 targetLocalEuler, float time)
        {
            if (time <= 0)
            {
                targetTsf.localEulerAngles = targetLocalEuler;
                yield break;
            }

            float timer = 0;

            Quaternion startQuaternion = targetTsf.localRotation;
            Quaternion targetQuaternion = Quaternion.Euler(targetLocalEuler);
            while (true)
            {
                while (IsPause)
                {
                    yield return null;
                }

                timer += Time.deltaTime;

                if (timer > time)
                {
                    targetTsf.localEulerAngles = targetLocalEuler;
                    yield break;
                }
                else
                {
                    targetTsf.localRotation = Quaternion.Lerp(startQuaternion, targetQuaternion, timer / time);
                }

                yield return null;
            }
        }

        private IEnumerator DoMove(Transform targetTsf, Vector3 targetLocalPosition, float time)
        {
            if (time <= 0)
            {
                targetTsf.localPosition = targetLocalPosition;
                yield break;
            }

            float timer = 0;

            Vector3 startLocalPosition = targetTsf.localPosition;

            while (true)
            {
                do
                {
                    yield return null;
                } while (IsPause);

                timer += Time.deltaTime;

                if (timer > time)
                {
                    targetTsf.localPosition = targetLocalPosition;
                    yield break;
                }
                else
                {
                    targetTsf.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, timer / time);
                }
            }
        }
    }
}