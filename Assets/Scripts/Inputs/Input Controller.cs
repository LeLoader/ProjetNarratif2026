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

    [Tooltip("Take a WILD guess")]
    [SerializeField] private bool _showDebug = true;

    private GameObject _target;

    private Vector2 _previousPosition;

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

    public void StartMove(InputAction.CallbackContext context)
    {
        Debug.Log("[INPUT CONTROLLER] start moving");
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_showDebug)
        {
            Debug.Log("[INPUT CONTROLLER] performing move");
        }
        if (Touch.activeTouches.Count >= 2)
        {
            if (_showDebug)
            {
                Debug.Log($"[INPUT CONTROLLER] not moving");
            }
            return;
        }
        Vector2 Input = context.ReadValue<Vector2>();
        if (_target != null)
        {
            if (_showDebug)
            {
                Debug.Log("[INPUT CONTROLLER] moving target");
            }

            Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input);
            WorldPosition.z = _target.transform.position.z;
            _target.transform.position = WorldPosition;

        } else
        {
            if (_showDebug)
            {
                Debug.Log($"[INPUT CONTROLLER] Input is {Input}");
            }
            if (_previousPosition != Vector2.zero)
            {
                Vector2 DeltaPosition = Input - _previousPosition;
                Vector3 CameraPosition = transform.position;
                CameraPosition.x += DeltaPosition.x * _movementSpeed * 0.01f;
                CameraPosition.y += DeltaPosition.y * _movementSpeed * 0.01f;
                transform.position = CameraPosition;                
            }
        }
        _previousPosition = Input;
    }

    public void EndMove(InputAction.CallbackContext context)
    {
        if (_showDebug)
        {
            Debug.Log("[INPUT CONTROLLER] stop moving");
        }
        if (transform.position != Camera.main.transform.position)
        {
            transform.position = Camera.main.transform.position;
        }
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        if (Touch.activeTouches.Count < 2)
        {
            if (_showDebug)
            {
                Debug.Log($"[INPUT CONTROLLER] stop zooming");
            }
            return;
        }

        // get the inputs
        Touch primary = Touch.activeTouches[0];
        Touch secondary = Touch.activeTouches[1];
        if (primary.history.Count < 1 || secondary.history.Count < 1)
        {
            if (_showDebug)
            {
                Debug.Log($"[INPUT CONTROLLER] no history");
            }
            return;
        }

        float currentDistance = Vector2.Distance(primary.screenPosition, secondary.screenPosition);
        float previousDistance = Vector2.Distance(primary.history[0].screenPosition, secondary.history[0].screenPosition);
        if (_showDebug)
        {
            Debug.Log($"[INPUT CONTROLLER] zooming");
        }

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

    public void CheckMoveTarget(InputAction.CallbackContext context)
    {
        if (_showDebug)
        {
            Debug.Log("[INPUT CONTROLLER] starting touch");
        }
        Vector2 InputValue = Touch.activeTouches[0].screenPosition;
        if (_showDebug)
        {
            Debug.Log($"[INPUT CONTROLLER] Input Value is {InputValue}");
        }
        RaycastHit HitResult;
        Vector3 StartPos = Camera.main.ScreenToWorldPoint(InputValue);
        StartPos.z = transform.position.z;
        if (_showDebug)
        {
            Debug.Log($"[INPUT CONTROLLER] StartPos is {StartPos}");
            //Debug.DrawLine(StartPos, StartPos + transform.forward * 100, Color.aliceBlue, 20f);
        }
        if (Physics.Raycast(StartPos, transform.forward, out HitResult, Mathf.Infinity))
        {
            if(HitResult.collider.gameObject.TryGetComponent<IMovable>(out IMovable target))
            {
                _target = HitResult.collider.gameObject;
                if (_showDebug)
                {
                    Debug.Log("[INPUT CONTROLLER] Movable Object Detected");
                }
            }
        }
    }

    public void CancelTouch(InputAction.CallbackContext context)
    {
        if (_showDebug)
        {
            Debug.Log("[INPUT CONTROLLER] Canceling touch");
        }
        _previousPosition = Vector2.zero;
        _target = null;
    }
}
