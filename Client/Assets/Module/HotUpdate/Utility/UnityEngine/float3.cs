using System;

namespace UnityEngine
{
    public class float3: float2
    {
        public float z;

        public float3() : base()
        {
            this.z = 0f;
        }

        public float3(float x) : base(x)
        {
            this.z = 0f;
        }

        public float3(float x, float y): base(x, y)
        {
            this.z = 0f;
        }

        public float3(float x, float y, float z) : base(x, y)
        {
            this.z = z;
        }

        public new float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new IndexOutOfRangeException("Invalid float3 index!"),
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
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid float3 index!");
                }
            }
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
