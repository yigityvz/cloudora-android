using UnityEngine;

namespace Cloudora.Core
{
    public static class MobileRuntimeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Application.backgroundLoadingPriority = ThreadPriority.Low;
        }
    }
}
