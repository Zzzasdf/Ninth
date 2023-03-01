using Cysharp.Threading.Tasks;
namespace Ninth.HotUpdate
{
    public abstract class HandlerTest_Json<TModel> : IHandlerTest<TModel>
        where TModel : ModelTest
    {
        public override async UniTask<TModel> Set()
        {
            TModel model = Model();
            model.AAAA = 11;
            return model;
        }

        public override async UniTask<TModel> Store()
        {
            TModel model = Model();
            await JsonProxy.Store(model);
            return model;
        }
    }
}