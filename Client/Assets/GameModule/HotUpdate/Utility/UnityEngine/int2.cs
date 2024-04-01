using System;

namespace UnityEngine
{
    public class int2
    {
        public int x;
        public int y;

        public int2()
        {
            this.x = 0;
            this.y = 0;
        }

        public int2(int x)
        {
            this.x = x;
            this.y = 0;
        }

        public int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException("Invalid int2 index!"),
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
                        throw new IndexOutOfRangeException("Invalid int2 index!");
                }
            }
        }

        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(x, y);
        }
    }
}
