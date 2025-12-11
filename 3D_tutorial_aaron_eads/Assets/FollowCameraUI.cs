using UnityEngine;

public class FollowCameraUI : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraTransform;   // Drag your Main Camera here
    public float distance = 0.6f;       // How far in front of the camera
    public float heightOffset = 0.0f;   // Optional vertical offset
    public float followSpeed = 10f;     // How quickly it locks onto the camera

    void Start()
    {
        // If no camera manually assigned, auto-assign Main Camera
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        // Position directly in front of the camera
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distance;
        targetPosition.y += heightOffset;

        // Instantly move (or smooth with Lerp for subtle delay)
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Always face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
