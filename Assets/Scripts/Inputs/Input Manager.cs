using EditorAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    [SerializeField, VerticalGroup("Input Actions References", true, nameof(_moveAction), nameof(_zoomAction))] private Void groupHolder;

    [SerializeField, HideProperty] private InputActionReference _moveAction;
    [SerializeField, HideProperty] private InputActionReference _zoomAction;

    [SerializeField] private InputController _controller;

    private void OnEnable()
    {
        _moveAction.action.performed += _controller.Move;
        _moveAction.action.canceled += _controller.EndMove;
        _zoomAction.action.performed += _controller.Zoom;
        Debug.Log("Move Bound");
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= _controller.Move;
        _moveAction.action.canceled -= _controller.EndMove;
        _zoomAction.action.performed -= _controller.Zoom;
        Debug.Log("Move Unbound");
    }
}
