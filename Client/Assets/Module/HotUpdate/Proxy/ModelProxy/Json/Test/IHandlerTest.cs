using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public abstract class IHandlerTest<TModel>
        where TModel : IModel
    {
        protected abstract TModel Model();

        public abstract UniTask<TModel> Store();

        public abstract UniTask<TModel> Set();
    }
}