using System;

namespace UnityEngine
{
    public class float4 : float3
    {
        public float w;

        public float4() : base()
        {
            this.w = 0f;
        }

        public float4(float x) : base(x)
        {
            this.w = 0f;
        }

        public float4(float x, float y) : base(x, y)
        {
            this.w = 0f;
        }

        public float4(float x, float y, float z) : base(x, y, z)
        {
            this.w = 0;
        }

        public float4(float x, float y, float z, float w):base(x, y, z)
        {
            this.w = w;
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
                    3 => w,
                    _ => throw new IndexOutOfRangeException("Invalid float4 index!"),
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
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid float4 index!");
                }
            }
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }
}
