using System.Threading;
using Cysharp.Threading.Tasks;
using Ninth.HotUpdate;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public interface IAssetProxy
    {
        public UniTask<T> ViewLoadAsync<T>(CancellationToken cancellationToken = default) where T : IView;
   
        public UniTask<GameObject> CloneAsync(string assetPath, CancellationToken cancellationToken = default);
        public UniTask<T> LoadAssetAsync<T>(string pathConfig, GameObject mountObj) where T : UnityEngine.Object;
        public UniTask UnLoadAllAsync();
    }
}