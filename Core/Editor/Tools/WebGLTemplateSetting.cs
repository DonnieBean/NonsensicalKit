using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/// <summary>
/// ��webgl�������Զ�̬�޸��ļ�����ʱδʹ��
/// </summary>
public class WebGLTemplateSetting : EditorWindow
{
    /// <summary>
    /// Build��ɺ�Ļص�
    /// </summary>
    /// <param name="target">�����Ŀ��ƽ̨</param>
    /// <param name="pathToBuiltProject">���������·��</param>
    [PostProcessBuild(1)]
    public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("Build Success  ���ƽ̨: " + target + "  ���·��: " + pathToBuiltProject);

        //���ļ����ļ���
        //System.Diagnostics.Process.Start(pathToBuiltProject);

        int index = pathToBuiltProject.LastIndexOf("/");

        Debug.Log("���������Ŀ¼ :" + pathToBuiltProject.Substring(0, index));
    }
}
