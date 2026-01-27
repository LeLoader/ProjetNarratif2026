using EditorAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    [SerializeField, VerticalGroup("Input Actions References", true, nameof(_moveAction), nameof(_zoomAction), nameof(_touchAction))] private Void groupHolder;

    [SerializeField, HideProperty] private InputActionReference _moveAction;
    [SerializeField, HideProperty] private InputActionReference _zoomAction;
    [SerializeField, HideProperty] private InputActionReference _touchAction;

    [SerializeField] private InputController _controller;

    private void OnEnable()
    {
        //_moveAction.action.started += _controller.StartMove;
        _moveAction.action.performed += _controller.Move;
        _moveAction.action.canceled += _controller.EndMove;
        _zoomAction.action.performed += _controller.Zoom;
        _touchAction.action.started += _controller.CheckMoveTarget;
        _touchAction.action.canceled += _controller.CancelTouch;
        Debug.Log("[INPUT MANAGER] Move Bound");
    }

    private void OnDisable()
    {
        //_moveAction.action.started += _controller.StartMove;
        _moveAction.action.performed -= _controller.Move;
        _moveAction.action.canceled -= _controller.EndMove;
        _zoomAction.action.performed -= _controller.Zoom;
        _touchAction.action.started -= _controller.CheckMoveTarget;
        _touchAction.action.canceled -= _controller.CancelTouch;
        Debug.Log("[INPUT MANAGER] Move Unbound");
    }
}
