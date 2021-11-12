using UnityEngine;

namespace NonsensicalKit.Utility
{

    /// <summary>
    /// Tansform������
    /// </summary>
    public static class TransformHelper 
    {

        /// <summary>
        /// ����·����ȡ��Ӧ���ӽڵ�
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s">ʹ��'|'�ָlike"1|2|3|5"</param>
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

