using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private BubbleController _bubble;
    [SerializeField] private Canvas _canvas;
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

        Debug.Log($"[CAMERA CONTROLLER] Before while. Alpha is {alpha}");
        while (alpha <= 1f)
        {
            alpha = alpha + Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, Destination, alpha);
            Debug.Log($"[CAMERA CONTROLLER] After While. Alpha is {alpha}");
            yield return null;
        }
        OnCompleteCallback?.Invoke();
    }


}
