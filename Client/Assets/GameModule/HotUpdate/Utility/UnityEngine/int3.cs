using System;

namespace UnityEngine
{
    public class int3 : int2
    {
        public int z;

        public int3() : base()
        {
            this.z = 0;
        }

        public int3(int x) : base(x)
        {
            this.z = 0;
        }

        public int3(int x, int y) : base(x, y)
        {
            this.z = 0;
        }

        public int3(int x, int y, int z) : base(x, y)
        {
            this.z = z;
        }

        public new int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new IndexOutOfRangeException("Invalid int3 index!"),
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
                        throw new IndexOutOfRangeException("Invalid int3 index!");
                }
            }
        }

        public Vector3Int ToVector3Int()
        {
            return new Vector3Int(x, y, z);
        }
    }
}
