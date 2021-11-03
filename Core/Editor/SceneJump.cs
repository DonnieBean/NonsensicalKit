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
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
        }
    }
}