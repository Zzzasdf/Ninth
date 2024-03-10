namespace Ninth.Utility
{
    public class PackMarkBit<TKey>
    {
        public TKey Key { get; }
        public int MarkBit { get; }
        public bool IsModify { get; }
        
        public PackMarkBit(TKey key, int markBit = 0, bool isModify = true)
        {
            this.Key = key;
            this.MarkBit = markBit;
            this.IsModify = isModify;
        }
    }
}