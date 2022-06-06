using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{
    /// <summary>
    /// 编辑器总管理类，用于管理一些通用但使用较消耗性能的属性
    /// </summary>
    public static class NonsensicalEditorManager
    {
        public static GameObject[] selectGameObjects;
        public static Transform selectTransform;
        public static Action selectChanged;
        public static Action EditorUpdate;

        [InitializeOnLoadMethod]
        private static void App()
        {
            EditorApplication.update += () =>
            {
                EditorUpdate?.Invoke();
            };

            Selection.selectionChanged += () =>
            {
                if (Selection.gameObjects.Length < 1)
                {
                    selectGameObjects = new GameObject[0];
                    selectTransform = null;
                }
                else
                {
                    selectGameObjects = Selection.gameObjects;
                    selectTransform = selectGameObjects[0].transform;
                }

                selectChanged?.Invoke();
            };
        }
    }

    public class Lab
    {
        /// <summary>
        /// 检测Resources文件夹内是否存在同名文件（无视后缀名）
        /// </summary>
        /// <returns></returns>
        [MenuItem("Tools/NonsensicalKit/Items/检测资源重名")]
        private static bool CheckResoureDuplicateName()
        {
            List<string> duplicateNameInfo = new List<string>();

            HashSet<string> vs = new HashSet<string>();

            Queue<DirectoryInfo> directoryInfos = new Queue<DirectoryInfo>();

            DirectoryInfo di = new DirectoryInfo(Application.dataPath + @"/Resources");

            directoryInfos.Enqueue(di);

            int leftCount = 1;

            while (leftCount > 0)
            {
                DirectoryInfo directoryInfo = directoryInfos.Dequeue();
                leftCount--;

                foreach (FileInfo item in directoryInfo.GetFiles())
                {
                    if (vs.Add(item.Name) == false)
                    {
                        duplicateNameInfo.Add(item.FullName);
                    }
                }

                foreach (DirectoryInfo item in directoryInfo.GetDirectories())
                {
                    directoryInfos.Enqueue(item);
                    leftCount++;
                }
            }

            foreach (var item in duplicateNameInfo)
            {
                Debug.Log($"资源重名：{item}");
            }

            if (duplicateNameInfo.Count == 0)
            {
                Debug.Log("无资源重名");
                return false;
            }

            return true;
        }

        [MenuItem("Tools/NonsensicalKit/Items/刷新项目文件")]
        private static void RefeshAsset()
        {
            AssetDatabase.Refresh();
            Debug.Log("刷新完成");
        }

        /// <summary>
        /// 根据名称排序场景内对象
        /// </summary>
        [MenuItem("Tools/NonsensicalKit/Items/根据名称排序")]
        private static void NameSort()
        {
            GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            for (int i = 0; i < roots.Length - 1; i++)
            {
                for (int j = i + 1; j < roots.Length; j++)
                {
                    if (string.Compare(roots[i].name, roots[j].name) > 0)
                    {
                        GameObject temp = roots[i];
                        roots[i] = roots[j];
                        roots[j] = temp;
                    }
                }
            }

            foreach (var item in roots)
            {
                item.transform.SetAsLastSibling();
            }

            Debug.Log("排序完成");
        }

    }
}