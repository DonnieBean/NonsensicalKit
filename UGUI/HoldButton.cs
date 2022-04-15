using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NonsensicalKit
{
    /// <summary>
    /// 长按一段时间才会触发的按钮    
    /// </summary>
    public class HoldButton : Selectable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Serializable]
        public class ButtonHoldEvent : UnityEvent { }

        [FormerlySerializedAs("onHold")]
        [SerializeField]
        private ButtonHoldEvent m_OnHold = new ButtonHoldEvent();

        [SerializeField]
        private float Interval = 0.5f;

        private float timer;

        private bool isHold = false;

        public ButtonHoldEvent OnHold
        {
            get { return m_OnHold; }
            set { m_OnHold = value; }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (isHold && timer > Interval)
            {
                m_OnHold.Invoke();
                timer = 0;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            timer = Interval;
            isHold = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            isHold = false;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            isHold = false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(HoldButton), true)]
    [CanEditMultipleObjects]
    public class HoldButtonEditor : SelectableEditor
    {
        SerializedProperty m_OnHoldProperty;
        SerializedProperty m_Interval;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnHoldProperty = serializedObject.FindProperty("m_OnHold");
            m_Interval = serializedObject.FindProperty("Interval");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_OnHoldProperty);
            EditorGUILayout.PropertyField(m_Interval);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}