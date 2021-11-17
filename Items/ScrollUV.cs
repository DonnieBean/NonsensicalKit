using UnityEngine;
namespace NonsensicalKit
{
    public class ScrollUV : NonsensicalMono
    {
        public enum Direction
        {
            Vertical, Horizontal
        }

        public float speed;
        public Direction direction;
        public Material material;
        private Vector2 offset;

        [SerializeField] private string stateSignal;
        private bool state=true;

        protected override void Awake()
        {
            base.Awake();
            Subscribe<bool>(stateSignal, (b)=> { state = b; });
        }

        private void Update()
        {
            if (state)
            {
                if (direction == Direction.Vertical)
                {
                    offset.y += speed * Time.deltaTime;
                    if (offset.y >= 1)
                    {
                        offset.y = 0;
                    }

                }
                else
                {
                    offset.x += speed * Time.deltaTime;
                    if (offset.x >= 1)
                    {
                        offset.x = 0;
                    }
                }

                material.mainTextureOffset = offset;
            }
        }
    }
}
