using EditorAttributes;
//using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch =  UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputController : MonoBehaviour
{
    [SerializeField, VerticalGroup("Speed Values", true, nameof(_movementSpeed), nameof(_zoomSpeed))] private Void groupHolder;

    [SerializeField, HideProperty, Range(0f, 20f)] private float _movementSpeed = 1f;
    [SerializeField, HideProperty, Range(0f, 20f)] private float _zoomSpeed = 1f;

  //  [SerializeField, Required] private CinemachineCamera _camera;

    [Tooltip("Zooming with the Camera Fov if true, or by moving the camera if false")]
    [SerializeField] private bool _isFovZoom;

    private void Reset()
    {
    //    TryGetComponent<CinemachineCamera>(out _camera);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnableTouchSupport()
    {
        if (!EnhancedTouchSupport.enabled)
        {
            EnhancedTouchSupport.Enable();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (Touch.activeTouches.Count >= 2)
        {
            Debug.Log($"[INPUT CONTROLLER] not moving");
            return;
        }
        Vector2 MoveValue = context.ReadValue<Vector2>();
        Vector3 CameraPosition = transform.position;
        CameraPosition.x -= MoveValue.x * _movementSpeed * 0.01f;
        CameraPosition.y -= MoveValue.y * _movementSpeed * 0.01f;
        transform.position = CameraPosition;
    }

    public void EndMove(InputAction.CallbackContext context)
    {
        if (transform.position != Camera.main.transform.position)
        {
            transform.position = Camera.main.transform.position;
        }
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        if (Touch.activeTouches.Count < 2)
        {
            Debug.Log($"[INPUT CONTROLLER] stop zooming");
            return;
        }

        // get the inputs
        Touch primary = Touch.activeTouches[0];
        Touch secondary = Touch.activeTouches[1];
        if (primary.history.Count < 1 || secondary.history.Count < 1)
        {
            Debug.Log($"[INPUT CONTROLLER] no history");
            return;
        }

        float currentDistance = Vector2.Distance(primary.screenPosition, secondary.screenPosition);
        float previousDistance = Vector2.Distance(primary.history[0].screenPosition, secondary.history[0].screenPosition);
        Debug.Log($"[INPUT CONTROLLER] zooming");

        float ZoomDistance = currentDistance - previousDistance;
        if (_isFovZoom)
        {
            //_camera.Lens.FieldOfView -= ZoomDistance * 0.1f * _zoomSpeed;
        } else
        {
            Vector3 CurrentPos = transform.position;
            CurrentPos.z = CurrentPos.z + ZoomDistance * 0.1f * _zoomSpeed;
            transform.position = CurrentPos;
        }
        //Debug.Log($"[INPUT FACTORY] first phase is {primary.phase} and second phase is {secondary.phase}");

    }

    public void StopZoom(InputAction.CallbackContext context)
    {
        Debug.Log("[INPUT CONTROLLER] cancelling zoom");
    }
}
