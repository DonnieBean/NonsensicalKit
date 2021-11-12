using UnityEngine;

namespace NonsensicalKit.Utility
{

    public class QuaternionHelper : MonoBehaviour
    {
        public bool CheckAngle(Transform tsf1, Transform tsf2, int checkDirCount, float allowAngel)
        {
            Quaternion q1 = Quaternion.LookRotation(tsf2.forward, tsf2.up);
            Quaternion q2 = Quaternion.FromToRotation(-tsf2.forward, tsf2.up);
            Quaternion q3 = Quaternion.FromToRotation(tsf2.right, tsf2.up);
            Quaternion q4 = Quaternion.FromToRotation(-tsf2.right, tsf2.up);

            Quaternion q5 = tsf1.rotation;

            switch (checkDirCount)
            {
                case 4:
                    {
                        if (Quaternion.Angle(q3, q5) <= allowAngel)
                        {
                            return true;
                        }
                        if (Quaternion.Angle(q4, q5) <= allowAngel)
                        {
                            return true;
                        }
                        goto case 2;
                    }
                case 2:
                    {
                        if (Quaternion.Angle(q2, q5) <= allowAngel)
                        {
                            return true;
                        }
                        goto case 1;
                    }
                case 1:
                    {
                        if (Quaternion.Angle(q1, q5) <= allowAngel)
                        {
                            return true;
                        }
                        return false;
                    }
                default: return true;
            }
        }
    }

}