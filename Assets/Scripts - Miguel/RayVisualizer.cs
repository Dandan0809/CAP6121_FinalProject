using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayVisualizer : MonoBehaviour
{
    public float maxDistance = 10f;
    public LayerMask raycastLayers; // Optional: filter what you hit
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.blue;
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Ray ray = new Ray(origin, direction);
        Vector3 endPoint = origin + direction * maxDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, raycastLayers))
        {
            endPoint = hit.point;
        }

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);
    }
}