using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MetricVisualizer : MonoBehaviour
{
    [Header("Meshes")]
    [SerializeField] private MeshRenderer indoctrinatedMesh;
    [SerializeField] private MeshRenderer freedomMesh;
    [SerializeField] private MeshRenderer violenceMesh;
    [SerializeField] private MeshRenderer peaceMesh;


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
        UpdateMeshRenderer(type, newState);
    }

    private void UpdateMeshRenderer(EMetricType type, EMetricState newState)
    {
        switch (type)
        {
            case EMetricType.INDOCTRINATED:
                switch (newState)
                {
                    case EMetricState.NEUTRAL:
                        freedomMesh.enabled = false;
                        indoctrinatedMesh.enabled = false;
                        break;
                    case EMetricState.POSITIVE:
                        freedomMesh.enabled = true;
                        indoctrinatedMesh.enabled = false;
                        break;
                    case EMetricState.NEGATIVE:
                        freedomMesh.enabled = false;
                        indoctrinatedMesh.enabled = true;
                        break;
                }
                break;
            case EMetricType.VIOLENCE:
                switch (newState)
                {
                    case EMetricState.NEUTRAL:
                        peaceMesh.enabled = false;
                        violenceMesh.enabled = false;
                        break;
                    case EMetricState.POSITIVE:
                        peaceMesh.enabled = true;
                        violenceMesh.enabled = false;
                        break;
                    case EMetricState.NEGATIVE:
                        peaceMesh.enabled = false;
                        violenceMesh.enabled = true;
                        break;
                }
                break;
        }
    }

    //private MeshRenderer GetSubtype(EMetricState newState)
    //{
    //    switch (newState)
    //    {
    //        case EMetricState.NEUTRAL:
    //            return null;
    //        case EMetricState.POSITIVE:
    //            return positiveMaterial;
    //        case EMetricState.NEGATIVE:
    //            return negativeMaterial;
    //        default:
    //            return null;
    //    }
    //}
}