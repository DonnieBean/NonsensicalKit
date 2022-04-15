using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public enum Axis { X, Y, Z }

    public class PincherFingerController : MonoBehaviour
    {
        [SerializeField] private Axis axis;
        private ArticulationBody articulation;
        List<float> buffer;
        int trueIndex;

        float min;
        float max;

        void Awake()
        {
            articulation = GetComponent<ArticulationBody>();
            buffer = new List<float>();


            var indexs = new List<int>();
            articulation.GetDofStartIndices(indexs);
            trueIndex = indexs[articulation.index];

            switch (axis)
            {
                case Axis.X:
                    min = articulation.xDrive.lowerLimit;
                    max = articulation.xDrive.upperLimit;
                    break;
                case Axis.Y:
                    min = articulation.yDrive.lowerLimit;
                    max = articulation.yDrive.upperLimit;
                    break;
                case Axis.Z:
                    min = articulation.zDrive.lowerLimit;
                    max = articulation.zDrive.upperLimit;
                    break;
            }
        }


        /// <summary>
        /// 通过当前位置返回当前插值
        /// </summary>
        /// <returns></returns>
        public float CurrentGrip()
        {
            articulation.GetDriveTargets(buffer);

            return Mathf.InverseLerp(min, max, buffer[trueIndex]);
        }

        /// <summary>
        /// 使用插值更新最新的Z Driver
        /// </summary>
        /// <param name="grip"></param>
        public void UpdateGrip(float grip)
        {
            articulation.GetDriveTargets(buffer);
            buffer[trueIndex] = Mathf.Lerp(min, max, grip);
            articulation.SetDriveTargets(buffer);
        }
    }

}