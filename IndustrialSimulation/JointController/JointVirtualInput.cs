using UnityEngine;

namespace NonsensicalKit.Joint
{
    public class JointVirtualInput : MonoBehaviour
    {
        [SerializeField] private JointController target;

        [SerializeField] private float interval = 1;

        [SerializeField] private float[] virtualValues;

        private float timer;

        private void Awake()
        {
#if !UNITY_EDITOR
        Destroy(gameObject);
        return;
#endif
            if (target)
            {
                int num = target.joints.Length;

                virtualValues = new float[num];

                for (int i = 0; i < num; i++)
                {
                    virtualValues[i] = target.joints[i].InitialValue;
                }
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                timer -= 1;

                if (target)
                {
                    ActionData ad = new ActionData(virtualValues, interval);

                    target.ChangeState(ad);
                }
            }
        }
    }
}