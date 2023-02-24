using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public class HandlerJsonTest<TModel> : IHandlerTest<TModel>
        where TModel : ModelTest
    {
        public TModel Model { get ; protected set; }

        public async UniTask<TModel> Get()
        {
            return Model;
        }

        public async UniTask<TModel> Set()
        {
            Model.AAAA = 10;
            return Model;
        }
    }
}