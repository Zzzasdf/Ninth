using UnityEngine;

namespace Ninth
{
    public partial class GameEntry : MonoBehaviour
    {
        public static DownloadCore DownloadCore;

        private void Awake()
        {
            DownloadCore = new DownloadCore();
        }

        void Start()
        {
            new Launcher().EnterProcedure();
        }
    }
}