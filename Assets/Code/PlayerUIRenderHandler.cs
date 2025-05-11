using UnityEngine;
using UnityEngine.Serialization;

public class PlayerUIRenderHandler : MonoBehaviour
{
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Canvas reticleCanvas;

    private void Awake()
    {
        if (reticleCanvas != null && playerCamera != null)
        {
            reticleCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            reticleCanvas.worldCamera = playerCamera;
        }
    }
}