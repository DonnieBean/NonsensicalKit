using NonsensicalKit;
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
        /// �ؽڽڵ�
        /// </summary>
        public Transform jointsNode;
        /// <summary>
        /// ��Ҫ�ı����
        /// </summary>
        public AxisType axisType;
        /// <summary>
        /// ��Ҫ�ı�ķ���
        /// </summary>
        public DirType dirType;
        /// <summary>
        /// ������̬ʱ��ŷ����/������
        /// </summary>
        public Vector3 zeroState;
        /// <summary>
        /// ������̬�ĳ�ʼֵ
        /// </summary>
        public float InitialValue;
        /// <summary>
        /// ת���ʣ�һ��λ�ı���Ҫ�ı���ٽǶ�/λ�ƣ�
        /// </summary>
        public float conversionRate = 1;
    }

    public class ActionData
    {
        public ActionData()
        {

        }

        public ActionData(float[] values, float time=0)
        {
            Values = values;
            Time = time;
        }

        /// <summary>
        /// ÿ���ڵ����ֵ
        /// </summary>
        public float[] Values { get; set; }
        /// <summary>
        /// ����Ŀ��ؽ���Ҫ���
        /// </summary>
        public float Time { get; set; }

        public int Length
        {
            get
            {
                return Values.Length;
            }
        }

    }

    /// <summary>
    /// ʹ����ֵ����ģ�ͽڵ�λ�ƻ�����ת
    /// </summary>
    public class JointController : NonsensicalMono
    {
        public bool IsPause { get; set; }

        public JointSetting[] joints;

        public void ChangeStates(IEnumerable<ActionData> jds)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeStatesCoroutine(jds));
        }

        public void ChangeState(ActionData jd)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeStateCoroutine(jd));
        }


        /// <summary>
        /// ���ڱ༭�������µ��������
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
            int min = joints.Length > jd.Length ? joints.Length : jd.Length;

            for (int i = 0; i < min - 1; i++)
            {
                StartCoroutine(ChangeJoint(i, jd.Values[i], jd.Time));
            }

            yield return ChangeJoint(min - 1, jd.Values[min - 1], jd.Time);
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
                do
                {
                    yield return null;
                } while (IsPause);

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