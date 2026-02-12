using System;
using EditorAttributes;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputController : MonoBehaviour
{
    [SerializeField, VerticalGroup("Speed Values", true, nameof(_movementSpeed), nameof(_zoomScale))] private EditorAttributes.Void speedValuesHolder;

    [SerializeField, VerticalGroup("Min / Max Zoom Value", true, nameof(_minCamerasize), nameof(_maxCameraSize))] private EditorAttributes.Void zoomHolder;

    [SerializeField] private TMP_Text textCharacterName;

    [SerializeField, HideProperty, Range(0f, 20f)] private float _movementSpeed = 1f;
    [SerializeField, HideProperty, Range(0f, 20f)] private float _zoomScale = 1f;

    [SerializeField, Required] private CinemachineCamera _camera;

    [SerializeField, HideProperty, Range(0.1f, 10f)] private float _minCamerasize;
    [SerializeField, HideProperty, Range(10f, 30f)] private float _maxCameraSize;

    [Tooltip("Take a WILD guess")]
    [SerializeField] private bool _showDebug = true;

    [SerializeField] private CameraController _cameraController;

    [SerializeField] private LayerMask _mask;

    [SerializeField] private UnityEvent onStartTouch;

    private GameObject _target;

    private Vector2 _previousPosition = Vector2.zero;

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
        if (_showDebug)
        {
            //Debug.Log("[INPUT CONTROLLER] performing move");
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()), transform.forward * 1000, Color.red, 0.1f);
        }
        if (Touch.activeTouches.Count >= 2 || _cameraController.IsMovingCamera())
        {
            if (_showDebug)
            {
                //Debug.Log($"[INPUT CONTROLLER] not moving");
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
            Vector3 StartPos = Camera.main.ScreenToWorldPoint(Input);
            if (Physics.Raycast(StartPos, transform.forward, out RaycastHit HitResult, Mathf.Infinity, _mask))
            {
                Vector3 HitPoint = HitResult.point;
                HitPoint.y = _target.transform.position.y;
                _target.transform.position = HitPoint;
            }
            else
            {
                Debug.Log("rien touche");
            }

            /*Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input);
            Debug.Log($"[INPUT CONTROLLER] World Position is {WorldPosition}");
            WorldPosition.y = _target.transform.position.y;
            _target.transform.position = WorldPosition;*/

        }
        else
        {
            if (_previousPosition != Vector2.zero)
            {
                Vector2 DeltaPosition = _previousPosition - Input;
                Vector3 CameraPosition = transform.position;
                CameraPosition.x += DeltaPosition.x * _movementSpeed * 0.01f * _camera.Lens.OrthographicSize / 5;
                CameraPosition.z += DeltaPosition.y * _movementSpeed * 0.01f * _camera.Lens.OrthographicSize / 5;
                transform.position = CameraPosition;
            }
        }
        _previousPosition = Input;
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
        _camera.Lens.OrthographicSize -= ZoomDistance * _zoomScale * 0.1f;
        _camera.Lens.OrthographicSize = Mathf.Clamp(_camera.Lens.OrthographicSize, 5, _maxCameraSize);

    }

    public void CheckMoveTarget(InputAction.CallbackContext context)
    {
        onStartTouch?.Invoke();
        Vector2 InputValue = Touch.activeTouches[0].screenPosition;
        /*
                if (_showDebug)
                {
                    Debug.Log($"[INPUT CONTROLLER] Input Value is {InputValue}");
                }*/
        RaycastHit HitResult;
        Vector3 StartPos = Camera.main.ScreenToWorldPoint(InputValue);
        StartPos.z = transform.position.z;
        /*
                if (_showDebug)
                {
                    Debug.Log($"[INPUT CONTROLLER] StartPos is {StartPos}");
                }*/
        if (Physics.Raycast(StartPos, transform.forward, out HitResult, Mathf.Infinity))
        {
            if (HitResult.collider.gameObject.TryGetComponent<Movable>(out Movable target))
            {
                BehaviorController controller = HitResult.collider.gameObject.GetComponent<BehaviorController>();
                controller.StopAi();
                controller.CallTriggerAnimation("dragAndDrop");
                _target = HitResult.collider.gameObject;
                textCharacterName.text = _target.name;

                if (_showDebug)
                {
                    Debug.Log("[INPUT CONTROLLER] Movable Object Detected");
                }
            }
        }
        else
        {
            if (_showDebug)
            {
                Debug.Log("[INPUT CONTROLLER] no object detected");
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
        if (_target)
        {
            textCharacterName.text = "";
            BehaviorController controller = _target.GetComponent<BehaviorController>();
            controller.ResumeAi();
            controller.CallTriggerAnimation("idle");
        }
        _target = null;

        if (transform.position != Camera.main.transform.position)
        {
            transform.position = Camera.main.transform.position;
        }
    }
}
