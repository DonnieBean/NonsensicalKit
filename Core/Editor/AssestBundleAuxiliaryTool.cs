﻿using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace NonsensicalKit.Editor
{
    public class AssestBundleAuxiliaryTool : EditorWindow
    {

        AssestBundleAuxiliaryTool()
        {
            optionsEnumArray = Enum.GetValues(typeof(BuildAssetBundleOptions));
            options = new bool[optionsEnumArray.Length];
        }

        [MenuItem("Tools/NonsensicalKit/AssestBundle辅助工具")]
        static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(AssestBundleAuxiliaryTool));
        }

        Array optionsEnumArray;

        string buildPath;


        bool[] options;

        BuildTarget buildTarget;

        private void OnGUI()
        {
            buildPath = PlayerPrefs.GetString("nk_assestBundleAuxiliaryTool_buildPath", Path.Combine(Application.dataPath,"Editor","AssetBundles"));
            buildPath = EditorGUILayout.TextField("目标文件夹路径：", buildPath);
            PlayerPrefs.SetString("nk_assestBundleAuxiliaryTool_buildPath", buildPath);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("打包选项:");
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 300;
            int i = 0;
            foreach (BuildAssetBundleOptions item in optionsEnumArray)
            {
                options[i] = PlayerPrefs.GetInt("nk_assestBundleAuxiliaryTool_buildOption_" + item.ToString(), 0) == 1;
                options[i] = EditorGUILayout.Toggle(item.ToString(), options[i]);
                PlayerPrefs.SetInt("nk_assestBundleAuxiliaryTool_buildOption_" + item.ToString(), options[i]?1:0);
                i++;
            }
            EditorGUIUtility.labelWidth = originalValue;

            EditorGUILayout.Space();

            buildTarget = (BuildTarget)PlayerPrefs.GetInt("nk_assestBundleAuxiliaryTool_buildTarget", 5);
            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("目标平台：", buildTarget);
            PlayerPrefs.SetInt("nk_assestBundleAuxiliaryTool_buildTarget", (int)buildTarget);
            if (GUILayout.Button("打包"))
            {
                if (Directory.Exists(buildPath)==false)
                {
                    Directory.CreateDirectory(buildPath);
                }
                BuildAssetBundleOptions buildOption=0;

                int j = 0;
                foreach (BuildAssetBundleOptions item in optionsEnumArray)
                {
                    if (options[j])
                    {
                        buildOption |= item;
                    }
                    j++;
                }

                Debug.Log(buildTarget.ToString());
                BuildPipeline.BuildAssetBundles(buildPath, buildOption, buildTarget);

                AssetDatabase.Refresh();
                Debug.Log("打包完成");
            }

            if (GUILayout.Button("ClearCache"))
            {
                Caching.ClearCache();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("设置包名"))
            {
                CheckFileSystemInfo();

                Debug.Log("设置完成");
            }
        }

        private static void CheckFileSystemInfo()  //检查目标目录下的文件系统
        {
            AssetDatabase.RemoveUnusedAssetBundleNames(); //移除没有用的assetbundlename
            UnityEngine.Object obj = Selection.activeObject;    // Selection.activeObject 返回选择的物体
            string path = AssetDatabase.GetAssetPath(obj);//选中的文件夹
            CoutineCheck(path);
        }

        private static void CheckFileOrDirectory(FileSystemInfo fileSystemInfo, string path) //判断是文件还是文件夹
        {
            FileInfo fileInfo = fileSystemInfo as FileInfo;
            if (fileInfo != null)
            {
                SetBundleName(path);
            }
            else
            {
                CoutineCheck(path);
            }
        }

        private static void CoutineCheck(string path)   //是文件夹，继续向下
        {
            DirectoryInfo directory = new DirectoryInfo(@path);
            FileSystemInfo[] fileSystemInfos = directory.GetFileSystemInfos();

            foreach (var item in fileSystemInfos)
            {
                // Debug.Log(item);
                int idx = item.ToString().LastIndexOf(@"\");//得到最后一个'\'的索引
                string name = item.ToString().Substring(idx + 1);//截取后面的作为名称

                if (!name.Contains(".meta"))
                {
                    CheckFileOrDirectory(item, path + "/" + name);  //item  文件系统，加相对路径
                }
            }
        }

        private static void SetBundleName(string path)  //设置assetbundle名字
        {
            var importer = AssetImporter.GetAtPath(path);
            string[] strs = path.Split('.');
            string[] dictors = strs[0].Split('/');
            string name = "";

             name = dictors[dictors.Length - 1];

            if (importer != null)
            {
                importer.assetBundleName = name;
                importer.assetBundleVariant = "assetBundle";
            }
            else
                Debug.Log("importer是空的");
        }
    }
}