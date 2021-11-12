using UnityEngine;

namespace NonsensicalKit.Utility
{

    /// <summary>
    /// Tansform工具类
    /// </summary>
    public static class TransformHelper 
    {

        /// <summary>
        /// 根据路径获取对应的子节点
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s">使用'|'分割，like"1|2|3|5"</param>
        /// <returns></returns>
        public static Transform GetTransformByNodePath(Transform root, string path)
        {

            Transform crt = root;

            string[] pathNode = path.Split('|');

            foreach (var node in pathNode)
            {
                int num;
                if (int.TryParse(node,out num))
                {
                    if (crt.childCount>num)
                    {
                        crt = crt.GetChild(num);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return crt;
        }

     
    }
}

