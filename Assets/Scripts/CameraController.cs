using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private BubbleController _bubble;
    [SerializeField] private Canvas _canvas;

    bool _isMovingCamera;
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
        _newBubble.SetCamera(this);
    }  

    public void GoToDestination(Vector3 Destination, Action OnCompleteCallback)
    {
        StartCoroutine(GoToDestinationCoroutine(Destination, OnCompleteCallback));
    }

    public IEnumerator GoToDestinationCoroutine(Vector3 Destination, Action OnCompleteCallback)
    {
        Destination.z = transform.position.z;
        float alpha = 0f;

        while (alpha <= 1f)
        {
            _isMovingCamera = true;
            alpha = alpha + Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, Destination, alpha);
            yield return null;
        }
        OnCompleteCallback?.Invoke();
        _isMovingCamera = false;
    }

    public bool IsMovingCamera()
    {
        return _isMovingCamera;
    }
}
