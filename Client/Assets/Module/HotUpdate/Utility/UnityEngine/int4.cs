using System;

namespace UnityEngine
{
    public class int4 : int3
    {
        public int w;

        public int4() : base()
        {
            this.w = 0;
        }

        public int4(int x) : base(x)
        {
            this.w = 0;
        }

        public int4(int x, int y) : base(x, y)
        {
            this.w = 0;
        }

        public int4(int x, int y, int z) : base(x, y, z)
        {
            this.w = 0;
        }

        public int4(int x, int y, int z, int w) : base(x, y, z)
        {
            this.w = w;
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
                    3 => w,
                    _ => throw new IndexOutOfRangeException("Invalid int4 index!"),
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
                        throw new IndexOutOfRangeException("Invalid int4 index!");
                }
            }
        }
    }
}
