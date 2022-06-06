using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/// <summary>
/// 再webgl打包后可以动态修改文件，暂时未使用
/// </summary>
public class WebGLTemplateSetting : EditorWindow
{
    /// <summary>
    /// Build完成后的回调
    /// </summary>
    /// <param name="target">打包的目标平台</param>
    /// <param name="pathToBuiltProject">包体的完整路径</param>
    [PostProcessBuild(1)]
    public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("Build Success  输出平台: " + target + "  输出路径: " + pathToBuiltProject);

        //打开文件或文件夹
        //System.Diagnostics.Process.Start(pathToBuiltProject);

        int index = pathToBuiltProject.LastIndexOf("/");

        Debug.Log("导出包体的目录 :" + pathToBuiltProject.Substring(0, index));
    }
}
