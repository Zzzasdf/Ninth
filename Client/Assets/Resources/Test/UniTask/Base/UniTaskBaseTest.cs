using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class UniTaskBaseTest : MonoBehaviour
    {
        [Header("加载文本")] [SerializeField] private Button btnLoadTxt;

        [Header("加载场景")] [SerializeField] private Button btnLoadScene;

        [Header("加载图片")] [SerializeField] private Button btnLoadTexture;

        [SerializeField] private Image imgTexture;

        private void Awake()
        {
            btnLoadTxt.onClick.AddListener(OnBtnLoadTextClick);
            btnLoadScene.onClick.AddListener(OnBtnLoadSceneClick);
            btnLoadTexture.onClick.AddListener(OnBtnLoadTextureClick);
        }

        public async void OnBtnLoadTextClick()
        {
            var loadOperation = Resources.LoadAsync<TextAsset>("Test/UniTask/test");
            var text = await loadOperation;
            (text as TextAsset)!.text.Log();
        }

        public async void OnBtnLoadSceneClick()
        {
            await LoadScene("Scenes/test");

            async UniTask LoadScene(string scenePath)
            {
                await SceneManager.LoadSceneAsync(scenePath).ToUniTask(
                    (Progress.Create<float>(
                        p => { $"读取进度{p * 100:F2}".Log(); })));
            }
        }

        public async void OnBtnLoadTextureClick()
        {
            var webRequest =
                UnityWebRequestTexture.GetTexture(
                    "https://i0.hdslb.com/bfs/static/jinkela/video/asserts/33-coin-ani.png");
            var result = await webRequest.SendWebRequest();
            var texture = ((DownloadHandlerTexture)result.downloadHandler).texture;
            int totalSpriteCount = 24;
            int perSpriteWidth = texture.width / totalSpriteCount;
            Sprite[] sprites = new Sprite[totalSpriteCount];
            for (int i = 0; i < totalSpriteCount; i++)
            {
                sprites[i] = Sprite.Create(texture,
                    new Rect(new Vector2(perSpriteWidth * i, 0), new Vector2(perSpriteWidth, texture.height)),
                    new Vector2(0.5f, 0.5f));
            }

            float perFrameTime = 0.1f;
            while (true)
            {
                for (int i = 0; i < totalSpriteCount; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(perFrameTime));
                    var sprite = sprites[i];
                    imgTexture.sprite = sprite;
                }
            }
        }
    }
}
