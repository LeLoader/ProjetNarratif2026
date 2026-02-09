using UnityEngine;

public class MetricVisualizer : MonoBehaviour
{
    [Header("Meshes")]
    [SerializeField] private MeshRenderer indoctrinatedMesh;
    [SerializeField] private MeshRenderer violenceMesh;

    [Header("Materials")]
    [SerializeField] private Material positiveMaterial;
    [SerializeField] private Material neutralMaterial;
    [SerializeField] private Material negativeMaterial;

    private void OnEnable()
    {
        GetComponent<BehaviorController>().OnMetricChanged += OnMetricChanged;
    }

    private void OnDisable()
    {
        GetComponent<BehaviorController>().OnMetricChanged -= OnMetricChanged;
    }

    public void OnMetricChanged(EMetricType type, EMetricState newState)
    {
        if (GetMeshRenderer(type)) GetMeshRenderer(type).material = GetMaterial(newState);
    }

    private MeshRenderer GetMeshRenderer(EMetricType type)
    {
        switch (type)
        {
            case EMetricType.INDOCTRINATED:
                return indoctrinatedMesh;
            case EMetricType.VIOLENCE:
                return violenceMesh;
            default:
                return null;
        }
    }

    private Material GetMaterial(EMetricState newState)
    {
        switch (newState)
        {
            case EMetricState.NEUTRAL:
                return neutralMaterial;
            case EMetricState.POSITIVE:
                return positiveMaterial;
            case EMetricState.NEGATIVE:
                return negativeMaterial;
            default:
                return neutralMaterial;
        }
    }
}