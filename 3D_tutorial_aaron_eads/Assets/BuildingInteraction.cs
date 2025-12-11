using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BuildingInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float rotationSpeed = 100f;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isGrabbed = false;
    private Vector3 previousControllerPosition;

    void Start()
    {
        // Get existing XR Grab Interactable (we added it manually in Inspector)
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on " + gameObject.name);
            return;
        }

        // Get or add rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = false;

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        previousControllerPosition = args.interactorObject.transform.position;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void Update()
    {
        if (isGrabbed && grabInteractable.interactorsSelecting.Count > 0)
        {
            // Get controller position
            Vector3 currentControllerPosition = grabInteractable.interactorsSelecting[0].transform.position;

            // Apply rolling ball rotation
            ApplyRollingBallRotation(currentControllerPosition);

            previousControllerPosition = currentControllerPosition;
        }
    }

    void ApplyRollingBallRotation(Vector3 controllerPosition)
    {
        // Calculate controller movement delta
        Vector3 delta = controllerPosition - previousControllerPosition;

        // Project onto horizontal plane
        delta.y = 0;

        if (delta.magnitude > 0.001f)
        {
            // Calculate rotation axis (perpendicular to movement)
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, delta).normalized;

            // Calculate rotation amount based on movement distance
            float rotationAmount = delta.magnitude * rotationSpeed * Time.deltaTime;

            // Apply rotation
            transform.Rotate(rotationAxis, rotationAmount, Space.World);
        }
    }
}
