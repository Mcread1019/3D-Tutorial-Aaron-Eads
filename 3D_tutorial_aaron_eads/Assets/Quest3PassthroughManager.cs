using UnityEngine;
using Unity.XR.CoreUtils;

public class Quest3PassthroughManager : MonoBehaviour
{
    [Header("Passthrough Settings")]
    [SerializeField] private bool enablePassthroughOnStart = true;

    private OVRPassthroughLayer passthroughLayer;

    void Start()
    {
        SetupPassthrough();
    }

    void SetupPassthrough()
    {
        // Add OVR Passthrough Layer to camera
        GameObject cameraObj = Camera.main.gameObject;
        passthroughLayer = cameraObj.AddComponent<OVRPassthroughLayer>();

        if (enablePassthroughOnStart)
        {
            EnablePassthrough();
        }
    }

    public void EnablePassthrough()
    {
        if (passthroughLayer != null)
        {
            passthroughLayer.enabled = true;
            // Make background transparent
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(0, 0, 0, 0);
        }
    }

    public void DisablePassthrough()
    {
        if (passthroughLayer != null)
        {
            passthroughLayer.enabled = false;
            Camera.main.clearFlags = CameraClearFlags.Skybox;
        }
    }
}