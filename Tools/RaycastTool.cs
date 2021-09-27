using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 保证同样的射线检测在同一帧只执行一次
    /// </summary>
    public class RaycastTool : MonoBehaviour
    {
        public static RaycastTool instance;

        public Camera raycastCamera;

        private Dictionary<string, RaycastHitsInfo> hitsInfo;
        private Dictionary<string, RaycastHitInfo> hitInfo;

        private void Awake()
        {
            instance = this;
            hitsInfo = new Dictionary<string, RaycastHitsInfo>();
            hitInfo = new Dictionary<string, RaycastHitInfo>();
        }

        public RaycastHit[] GetHits(string mask = "NULL")
        {
            int crtCount = Time.frameCount;
            if (hitsInfo.ContainsKey(mask) && hitsInfo[mask].FrameCount == crtCount)
            {
                return hitsInfo[mask].RaycastHits;
            }
            else
            {
                if (hitsInfo.ContainsKey(mask) == false)
                {
                    hitsInfo.Add(mask, null);
                }
                hitsInfo[mask] = CheckAll(mask);
                return hitsInfo[mask].RaycastHits;
            }
        }

        public RaycastHit GetHit(string mask = "NULL")
        {
            int crtCount = Time.frameCount;
            if (hitInfo.ContainsKey(mask) && hitInfo[mask].FrameCount == crtCount)
            {
                return hitInfo[mask].RaycastHit;
            }
            else
            {
                if (hitInfo.ContainsKey(mask) == false)
                {
                    hitInfo.Add(mask, null);
                }
                hitInfo[mask] = CheckFirst(mask);
                return hitInfo[mask].RaycastHit;
            }
        }

        private RaycastHitsInfo CheckAll(string mask)
        {
            RaycastHitsInfo raycastInfo = new RaycastHitsInfo();
            raycastInfo.FrameCount = Time.frameCount;
            Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

            if (mask == "NULL")
            {
                raycastInfo.RaycastHits = Physics.RaycastAll(ray, 100);
            }
            else
            {
                raycastInfo.RaycastHits = Physics.RaycastAll(ray, 100, LayerMask.GetMask(mask));
            }
            return raycastInfo;
        }

        private RaycastHitInfo CheckFirst(string mask)
        {
            RaycastHitInfo raycastInfo = new RaycastHitInfo();
            raycastInfo.FrameCount = Time.frameCount;

            Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

            if (mask == "NULL")
            {
                Physics.Raycast(ray, out raycastInfo.RaycastHit, 100);
            }
            else
            {
                Physics.Raycast(ray, out raycastInfo.RaycastHit, 100, LayerMask.GetMask(mask));
            }
            return raycastInfo;
        }
    }

    public class RaycastHitsInfo
    {
        public int FrameCount;
        public RaycastHit[] RaycastHits;
    }
    public class RaycastHitInfo
    {
        public int FrameCount;
        public RaycastHit RaycastHit;
    }

}
