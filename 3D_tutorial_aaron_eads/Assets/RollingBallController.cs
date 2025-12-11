using UnityEngine;

public class RollingBallController : MonoBehaviour
{
    [Header("Rolling Ball Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private Transform virtualSphere; // Optional visual sphere

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        // For Quest controller ray interaction
        HandleControllerInput();

        // Fallback for Unity editor testing with mouse
        if (Application.isEditor)
        {
            HandleMouseInput();
        }
    }

    void HandleControllerInput()
    {
        // This will be triggered by XR ray interactor
        // The XRGrabInteractable handles the actual grabbing
        // This is just for additional rolling ball logic
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 delta = currentMousePosition - lastMousePosition;

            // Map 2D mouse movement to 3D rotation
            float angleX = delta.y * sensitivity;
            float angleY = -delta.x * sensitivity;

            // Apply rotation
            transform.Rotate(Camera.main.transform.right, angleX, Space.World);
            transform.Rotate(Vector3.up, angleY, Space.World);

            lastMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
