using UnityEngine;

public class ProceduralBuilding : MonoBehaviour
{
    [Header("Building Parameters")]
    [SerializeField] private int floors = 5;
    [SerializeField] private float floorHeight = 3f;
    [SerializeField] private float buildingWidth = 8f;
    [SerializeField] private float buildingDepth = 8f;
    [SerializeField] private Material buildingMaterial;

    void Start()
    {
        GenerateBuilding();
    }

    void GenerateBuilding()
    {
        // Clear any existing children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float totalHeight = floors * floorHeight;

        // Create main building body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "BuildingBody";
        body.transform.SetParent(transform);
        body.transform.localPosition = new Vector3(0, totalHeight / 2f, 0);
        body.transform.localScale = new Vector3(buildingWidth, totalHeight, buildingDepth);

        // Apply material
        if (buildingMaterial != null)
        {
            body.GetComponent<MeshRenderer>().material = buildingMaterial;
        }

        // Disable the primitive's collider (we'll use parent's collider)
        Destroy(body.GetComponent<Collider>());

        // Create pyramid roof
        GameObject roof = new GameObject("Roof");
        roof.transform.SetParent(transform);
        roof.transform.localPosition = new Vector3(0, totalHeight, 0);

        // Create 4 triangular sides of the pyramid using cubes (rotated and scaled)
        float roofHeight = 2f;
        float roofScale = Mathf.Max(buildingWidth, buildingDepth);

        // Front face
        CreateRoofFace(roof.transform, new Vector3(0, roofHeight / 2f, buildingDepth / 4f),
                       new Vector3(0, 45, 0), new Vector3(buildingWidth, 0.1f, roofHeight));

        // Back face
        CreateRoofFace(roof.transform, new Vector3(0, roofHeight / 2f, -buildingDepth / 4f),
                       new Vector3(0, -45, 0), new Vector3(buildingWidth, 0.1f, roofHeight));

        // Left face
        CreateRoofFace(roof.transform, new Vector3(-buildingWidth / 4f, roofHeight / 2f, 0),
                       new Vector3(0, 0, 45), new Vector3(0.1f, roofHeight, buildingDepth));

        // Right face
        CreateRoofFace(roof.transform, new Vector3(buildingWidth / 4f, roofHeight / 2f, 0),
                       new Vector3(0, 0, -45), new Vector3(0.1f, roofHeight, buildingDepth));

        // Add windows as darker cubes on walls
        CreateWindows(totalHeight);

        Debug.Log($"Building generated: {floors} floors, {totalHeight}m tall");
    }

    void CreateRoofFace(Transform parent, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject face = GameObject.CreatePrimitive(PrimitiveType.Cube);
        face.transform.SetParent(parent);
        face.transform.localPosition = position;
        face.transform.localEulerAngles = rotation;
        face.transform.localScale = scale;

        if (buildingMaterial != null)
        {
            face.GetComponent<MeshRenderer>().material = buildingMaterial;
        }

        // Disable collider
        Destroy(face.GetComponent<Collider>());
    }

    void CreateWindows(float totalHeight)
    {
        // Create simple window decorations
        int windowsPerSide = 3;
        float windowSize = 0.5f;
        float windowSpacing = buildingWidth / (windowsPerSide + 1);

        for (int floor = 0; floor < floors; floor++)
        {
            float yPos = floor * floorHeight + floorHeight / 2f;

            // Front windows
            for (int i = 1; i <= windowsPerSide; i++)
            {
                CreateWindow(new Vector3(-buildingWidth / 2f + i * windowSpacing, yPos, buildingDepth / 2f + 0.05f), windowSize);
            }

            // Back windows
            for (int i = 1; i <= windowsPerSide; i++)
            {
                CreateWindow(new Vector3(-buildingWidth / 2f + i * windowSpacing, yPos, -buildingDepth / 2f - 0.05f), windowSize);
            }
        }
    }

    void CreateWindow(Vector3 position, float size)
    {
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
        window.name = "Window";
        window.transform.SetParent(transform);
        window.transform.localPosition = position;
        window.transform.localScale = new Vector3(size, size, 0.05f);

        // Make windows darker
        MeshRenderer mr = window.GetComponent<MeshRenderer>();
        Material windowMat = new Material(mr.material);
        windowMat.color = new Color(0.2f, 0.2f, 0.3f); // Dark blue-grey
        mr.material = windowMat;

        // Disable collider
        Destroy(window.GetComponent<Collider>());
    }

    // Regenerate when values change in editor
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            GenerateBuilding();
        }
    }
}