using System;
using System.Linq;
using System.Reflection;

namespace NonsensicalKit.Utility
{
    /// <summary>
    /// 反射工具类
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 根据class name反射获取Type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetTypeByTypeName(string typeName)
        {
            Assembly assembly=Assembly.GetExecutingAssembly();
            Type type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            if (type==null)
            {
                throw new Exception(string.Format("Cant't find Class by class name:'{0}'",typeName));
            }
            return type;
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetAssembly()
        {
            Assembly[] AssbyCustmList = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < AssbyCustmList.Length; i++)
            {
                string assbyName = AssbyCustmList[i].GetName().Name;
                if (assbyName == "Assembly-CSharp")
                {
                    return AssbyCustmList[i];
                }
            }
            return null;
        }
    }
}
