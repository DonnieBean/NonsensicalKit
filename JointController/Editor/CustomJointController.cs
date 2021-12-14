using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Joint
{
    [CustomEditor(typeof(JointController),true)]
    public class CustomButtonDrawer : UnityEditor. Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            JointController jc = (JointController)target;
            if (GUILayout.Button("ResetZeroState"))
            {
                jc.ResetZeroState();
                EditorUtility.SetDirty(jc);
            }
            if (GUILayout.Button("ResetRobotState"))
            {
                jc.ResetRobotState();
                EditorUtility.SetDirty(jc);
            }
        }
    }
}
