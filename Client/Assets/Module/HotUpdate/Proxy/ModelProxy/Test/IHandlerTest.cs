using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public interface IHandlerTest<TModel>
        where TModel : IModel
    {
        public TModel Model { get; }

        public UniTask<TModel> Get();

        public UniTask<TModel> Set();
    }
}