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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RepairRootCanvases()
        {
            Canvas[] canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include);
            foreach (Canvas canvas in canvases)
            {
                EnsureCanvasVisible(canvas);
            }
        }

        public static bool EnsureCanvasVisible(Canvas canvas)
        {
            if (canvas == null || !canvas.isRootCanvas)
            {
                return false;
            }

            Transform canvasTransform = canvas.transform;
            Vector3 scale = canvasTransform.localScale;
            if (Mathf.Abs(scale.x) > 0.001f && Mathf.Abs(scale.y) > 0.001f && Mathf.Abs(scale.z) > 0.001f)
            {
                return false;
            }

            canvasTransform.localScale = Vector3.one;
            Debug.LogWarning("Cloudora repaired an invisible root Canvas scale at startup.");
            return true;
        }
    }
}
