using System.Collections.Generic;
using UnityEngine;
using NonsensicalKit.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NonsensicalKit.Joint
{
    public class JointRecorder : MonoBehaviour, IUseProtocols<VirtualRobotData>
    {
        [SerializeField] private JointController jc;
        [SerializeField] private float Interval = 0.2f;
        public bool recording { get; private set; } = false;
        private float timer;
        private List<VirtualRobotData> recordData;

        private void Update()
        {
            if (recording)
            {
                timer += Time.deltaTime;
                if (timer>Interval)
                {
                    timer -= Interval;
                    recordData.Add(new VirtualRobotData(jc.GetJointsValue()));
                }
            }
        }

        public void StartRecord()
        {
            timer = Interval;
            recordData = new List<VirtualRobotData>();
            recording = true;
        }

        public void StopRecord()
        {
            recording = false;
            
            FileHelper.AutoWriteTxt(JsonHelper.SerializeObject(recordData));
        }

        public void OnReceivedMessage(VirtualRobotData value)
        {
            jc.ChangeState(new ActionData(value.data, 0.2f));
        }
    }
    public class VirtualRobotData
    {
        public float[] data;

        public VirtualRobotData(float[] data)
        {
            this.data = data;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(JointRecorder))]
    public class JointRecorderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            JointRecorder jc = (JointRecorder)target;
            if (jc.recording)
            {
                if (GUILayout.Button("StopRecord"))
                {
                    jc.StopRecord();
                }
            }
            else
            {
                if (GUILayout.Button("StartRecord"))
                {
                    jc.StartRecord();
                }
            }
        }
    }
#endif
}

