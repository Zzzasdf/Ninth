using System.Collections;
using UnityEngine;

namespace Ninth
{
    public partial class GameEntry : MonoBehaviour
    {
        public static DownloadCore DownloadCore;

        public static GameEntry Instance {  get ; private set; }

        private void Awake()
        {
            Instance = this;

            DownloadCore = new DownloadCore();
        }

        void Start()
        {
            new Launcher().EnterProcedure();
        }
    }
}