using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace Ninth.HotUpdate
{
    public partial class AssetsMgr
    {
        public async UniTask LoadSceneAsync(string path, IProgress<float> progress = null)
        {
            await SceneManager.LoadSceneAsync(path).ToUniTask(progress);
            UnLoadAllAsync();
        }

        public async void TestLoadSceneAsync()
        {
            IProgress<float> progress = Progress.Create<float>((p) =>
            {
                $"{p * 100:F2}%".Log("场景加载进度{0}");
            });

            await LoadSceneAsync("Assets/Scenes/HotUpdateScene.unity", progress);
        }
    }
}