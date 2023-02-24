using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public class HandlerTest2<TModel> : IHandlerTest<TModel>
        where TModel : ModelTest
    {
        public TModel Model { get; protected set; }

        public async UniTask<TModel> Get()
        {
            return Model;
        }

        public async UniTask<TModel> Set()
        {
            Model.AAAA = 20;
            return Model;
        }
    }
}

