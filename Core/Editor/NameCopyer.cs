using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{

    public class NameCopyer : EditorWindow
    {
        [MenuItem("Tools/NonsensicalKit/快速命名工具")]
        static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(NameCopyer));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("复制"))
            {
                copyBuffer = new NameTree();

                Copy(NonsensicalEditorManager.selectTransform, copyBuffer);

                Debug.Log("复制成功");
            }

            if (GUILayout.Button("粘贴"))
            {
                Paste(NonsensicalEditorManager.selectTransform, copyBuffer);
                Undo.RecordObject(NonsensicalEditorManager.selectTransform,"PasteName");
                Debug.Log("粘贴成功");
            }
        }

        private void Copy(Transform node, NameTree nameTree)
        {
            nameTree.name = node.name;
            nameTree.childs = new List<NameTree>();
            foreach (Transform item in node)
            {
                NameTree newChild = new NameTree();
                nameTree.childs.Add(newChild);
                Copy(item, newChild);
            }
        }

        private void Paste(Transform node, NameTree nameTree)
        {
            node.name = nameTree.name;

            int min = Mathf.Min(node.childCount, nameTree.childs.Count);
            for (int i = 0; i < min; i++)
            {
                Paste(node.GetChild(i), nameTree.childs[i]);
            }
        }

        NameTree copyBuffer;

        class NameTree
        {
            public string name;
            public List<NameTree> childs;
        }
    }

}
