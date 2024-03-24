namespace Ninth.HotUpdate
{
    public interface INode
    {
        public int UniqueId { get; set; }
        public int Weight { get; set; }
        public int OrderId { get; set; }
    }
}