using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private BubbleController _bubble;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _camera;
    private void OnEnable()
    {
        ACT_GoToPc.OnComputerReached += ShowBubble;
    }

    private void OnDisable()
    {
        ACT_GoToPc.OnComputerReached -= ShowBubble;
    }

    private void ShowBubble(Vector3 EventPosition, Action ActionToExecute)
    {
        BubbleController _newBubble = Instantiate(_bubble, _canvas.transform);
        _newBubble.SetDestination(EventPosition);
        _newBubble.OnClicked += ActionToExecute;
        
    }
}
