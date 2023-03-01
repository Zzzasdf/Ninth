namespace Ninth.HotUpdate
{
    public class ModelTest: HandlerTest_Json<ModelTest>, IModel
    {
        protected override ModelTest Model()
        {
            return this;
        }

        public int AAAA;

        public int DDDD;

        public int VVV;
    }
}