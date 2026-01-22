using EditorAttributes;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch =  UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _movementSpeed = 0.01f;

    [SerializeField, Required] private CinemachineCamera _camera;

    private bool _isZooming;

    private void Reset()
    {
        TryGetComponent<CinemachineCamera>(out _camera);
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
        if (_isZooming)
        {
            return;
        }
        Vector2 MoveValue = context.ReadValue<Vector2>();
        Vector3 CameraPosition = transform.position;
        CameraPosition.x -= MoveValue.x * _movementSpeed;
        CameraPosition.y -= MoveValue.y * _movementSpeed;
        transform.position = CameraPosition;
        //Debug.Log($"[INPUT CONTROLLER] move value = {MoveValue}");
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
            _isZooming = false;
            return;
        }
        _isZooming = true;

        // get the inputs
        Touch primary = Touch.activeTouches[0];
        Touch secondary = Touch.activeTouches[1];

        if (primary.phase == TouchPhase.Moved || secondary.phase == TouchPhase.Moved)
        {
            if (primary.history.Count < 1 || secondary.history.Count < 1)
            {
                return;
            }

            float currentDistance = Vector2.Distance(primary.screenPosition, secondary.screenPosition);
            float previousDistance = Vector2.Distance(primary.history[0].screenPosition, secondary.history[0].screenPosition);
            Debug.Log($"[INPUT FACTORY] zooming");

            float ZoomDistance = currentDistance - previousDistance;
            Vector3 CurrentPos = transform.position;
            CurrentPos.z = CurrentPos.z + ZoomDistance * 0.1f;
            //_camera.Lens.FieldOfView -= ZoomDistance * 0.1f;
            //CurrentPos.z = CurrentPos.z + ZoomDistance * 0.1f;
            //_camera.Lens.FieldOfView -= ZoomDistance * 0.1f;
            transform.position = CurrentPos;
        }
    }
}
