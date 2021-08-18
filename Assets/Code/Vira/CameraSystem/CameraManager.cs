using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VIRA.CameraSystem
{
    public class CameraManager : MonoBehaviourSingleton<CameraManager>
    {
        [SerializeField] private Camera gameplayCamera = default;
        [SerializeField] private Camera resultCamera = default;
        [SerializeField] private UniversalAdditionalCameraData gameplayData;
        [SerializeField] private UniversalAdditionalCameraData resultData;

        public Camera GameplayCamera => gameplayCamera;
        public Camera ResultCamera => resultCamera;
        public UniversalAdditionalCameraData GameplayCameraData => gameplayData;
        public UniversalAdditionalCameraData ResultCameraData => resultData;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameplayCamera != null)
            {
                gameplayData = gameplayCamera.GetUniversalAdditionalCameraData();
            }
            if (resultCamera != null)
            {
                resultData = resultCamera.GetUniversalAdditionalCameraData();
            }
        }

        [ContextMenu("Change Result Render Type")]
        private void ChangeResultRenderType()
        {
            resultData.renderType = resultData.renderType == CameraRenderType.Base ? CameraRenderType.Overlay : CameraRenderType.Base;
        }
#endif
    }
}