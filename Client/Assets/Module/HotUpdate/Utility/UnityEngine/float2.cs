using System;

namespace UnityEngine
{
    public class float2
    {
        public float x;
        public float y;

        public float2()
        {
            this.x = 0f;
            this.y = 0f;
        }

        public float2(float x)
        {
            this.x = x;
            this.y = 0f;
        }

        public float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException("Invalid float2 index!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid float2 index!");
                }
            }
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
}
