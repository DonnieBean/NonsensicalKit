using UnityEditor;

/// <summary>
/// ���ڸı�༭�����õĹ�����
/// </summary>
public static class EditorHelper
{
#if UNITY_EDITOR
    public static bool CheckScriptingDefine(string scriptingDefine)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        return defines.Contains(scriptingDefine);
    }

    public static void SetScriptingDefine(string scriptingDefine)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        if (!defines.Contains(scriptingDefine))
        {
            defines += $";{scriptingDefine}";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }
    }

    public static void RemoveScriptingDefine(string scriptingDefine)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        if (defines.Contains(scriptingDefine))
        {
            string newDefines = defines.Replace(scriptingDefine, "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefines);
        }
    }
#endif

}