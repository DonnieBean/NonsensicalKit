using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonsensicalKit.Editor
{
    [InitializeOnLoad]
    class SceneJump
    {
        static SceneJump()
        {
            EditorApplication.playModeStateChanged += PlayModeStateChange;
        }

        static void PlayModeStateChange(PlayModeStateChange playModeStateChange)
        {
            bool needJump = PlayerPrefs.GetInt("nk_nonsensicalConfigurator_jumpFirstOnPlay", 0)==0?false:true;
            if (needJump&& playModeStateChange == UnityEditor.PlayModeStateChange.EnteredPlayMode)
            {
                var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
                var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                clearMethod.Invoke(null, null);

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
        }
    }
}