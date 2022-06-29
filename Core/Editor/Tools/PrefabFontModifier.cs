using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �����޸�Ԥ���������
/// Ŀǰ�������⣬��Ҫ�޸Ķ�β��ܹ���ȫ�޸���ɣ����ؽ���Ŀ���Կ��ܳ���δ�޸ĵ����⣬�ƺ�ֻ��SetDirty�����ܺܺõı��������޸�
/// Ŀǰֻ��Ҫ�ظ��޸����޸�0������Ȼ��������Ŀ���ظ�������ֱ���������һ���޸���Ϊ0�μ��ɣ������������ڶ��ξ������
/// </summary>
public class PrefabFontModifier : EditorWindow
{
    [MenuItem("Tools/NonsensicalKit/�����޸�/Ԥ���������޸���")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PrefabFontModifier));
    }

    private static class PrefabFontModifierPanel
    {
        public static TMP_FontAsset font;
    }

    private void OnGUI()
    {

        PrefabFontModifierPanel.font = (TMP_FontAsset)EditorGUILayout.ObjectField("Font", PrefabFontModifierPanel.font, typeof(TMP_FontAsset), true, GUILayout.MinWidth(100f));

        if (GUILayout.Button("�޸�"))
        {
            //Test();
            string objPath = Application.dataPath;

            List<GameObject> prefabs = new List<GameObject>();

            var absolutePaths = System.IO.Directory.GetFiles(objPath, "*.prefab", System.IO.SearchOption.AllDirectories);

            for (int i = 0; i < absolutePaths.Length; i++)
            {
                EditorUtility.DisplayProgressBar("��ʾ", "��ȡԤ������...", (float)i / absolutePaths.Length);

                string path = "Assets" + absolutePaths[i].Remove(0, objPath.Length);
                path = path.Replace("\\", "/");

                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (prefab != null)
                    prefabs.Add(prefab);
                else
                    Debug.Log("Ԥ���岻���ڣ� " + path);
            }

            EditorUtility.ClearProgressBar();

            ChangeFont(prefabs, PrefabFontModifierPanel.font);
        }
    }

    private void Test()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.Log("δѡ���κζ���");
        }
        else
        {
            //for (int i = 0; i < Selection.gameObjects.Length; i++)
            //{
            //    Transform[] tsfs = Selection.gameObjects[i].GetComponentsInChildren<Transform>(true);

            //    foreach (var tsf in tsfs)
            //    {
            //        Debug.Log("!__________________!");
            //        Debug.Log(tsf.name);

            //        Debug.Log("IsPartOfPrefabInstance:          " + PrefabUtility.IsPartOfPrefabInstance(tsf));
            //        Debug.Log("IsAddedGameObjectOverride:       " + PrefabUtility.IsAddedGameObjectOverride(tsf.gameObject));
            //        Debug.Log("IsPartOfNonAssetPrefabInstance:  " + PrefabUtility.IsPartOfNonAssetPrefabInstance(tsf.gameObject));
            //        Debug.Log("IsPartOfVariantPrefab:           " + PrefabUtility.IsPartOfVariantPrefab(tsf.gameObject));
            //    }
            //}

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                TextMeshProUGUI[] texts = Selection.gameObjects[i].GetComponentsInChildren<TextMeshProUGUI>(true);

                foreach (var text in texts)
                {
                    Debug.Log("!__________________!");
                    Debug.Log(text.name + "______" + PrefabUtility.IsPartOfPrefabInstance(text));

                    var v = PrefabUtility.GetPropertyModifications(text);
                    if (v != null)
                    {
                        foreach (var item in v)
                        {
                            if (item.propertyPath == "m_fontAsset")
                                Debug.Log(item.propertyPath + ":" + item.value + ",IsDefaultOverride: " + PrefabUtility.IsDefaultOverride(item));
                        }
                    }
                }
            }
        }
    }

    private void ChangeFont(List<GameObject> prefabs, TMP_FontAsset font)
    {
        int count = 0;
        foreach (var prefab in prefabs)
        {
            TextMeshProUGUI[] texts = prefab.gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (var text in texts)
            {
                if (text.font != font)
                {
                    //ֻ�ᴦ���Ԥ���岿�֣���Ԥ���������õ�����Ԥ���岻���д���ֻ�ᴦ��ԭʼ���֣�
                    if (PrefabUtility.IsPartOfPrefabInstance(text.gameObject) == false)
                    {
                        Debug.Log("�޸���" + PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(text.gameObject) + "��Text���");
                        count++;
                        text.font = font;
                        EditorUtility.SetDirty(prefab);
                    }
                    //�������Ԥ���岿�ֵ������޸�����Ҫ���д�����Ϊ��ʱ�޸����õ�Ԥ����ʱ��������޸�
                    else
                    {
                        var v = PrefabUtility.GetPropertyModifications(text);
                        if (v != null)
                        {
                            bool flag = false;
                            foreach (var item in v)
                            {
                                if (item.propertyPath == "m_fontAsset")
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                Debug.Log("�޸���" + PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(text.gameObject) + "��Text���");
                                count++;
                                text.font = font;
                                EditorUtility.SetDirty(prefab);
                            }
                        }
                    }
                }
            }
        }
        EditorUtility.DisplayDialog("", "�����������,���޸���" + count + "������", "OK");
    }
}
