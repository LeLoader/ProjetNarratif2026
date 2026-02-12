using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour, IPointerClickHandler
{
    public event Action OnClicked;

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;
    [SerializeField] private CameraController _cameraController;

    private Vector3 _destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangePosition();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ChangePosition();
    }

    #region Setters

    public void SetDestination(Vector3 Destination)
    {
        _destination = Destination;
    }

    public void SetCamera(CameraController CameraController)
    {
        _cameraController = CameraController;
    }

    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[BUBBLE CONTROLLER] bubble clicked");
        Vector3 DestinationOnScreen = Camera.main.WorldToScreenPoint(_destination);
        if (IsDestinationOffScreen(DestinationOnScreen))
        {
            _cameraController.GoToDestination(_destination, OnClicked);
            Debug.Log("[BUBBLE CONTROLLER] destinationed");
        } else
        {
            OnClicked?.Invoke();
        }
        Destroy(gameObject);
    }

    private bool IsDestinationOffScreen(Vector3 OnScreenDestination)
    {
        return OnScreenDestination.x - _rectTransform.sizeDelta.x < 0 || OnScreenDestination.x + _rectTransform.sizeDelta.x > Screen.width || OnScreenDestination.y - _rectTransform.sizeDelta.y < 0 || OnScreenDestination.y + _rectTransform.sizeDelta.y > Screen.height;
    }

    private void ChangePosition()
    {
        Vector3 DestinationOnScreen = Camera.main.WorldToScreenPoint(_destination);
        if (IsDestinationOffScreen(DestinationOnScreen))
        {
            Vector3 CappedScreenPosition = DestinationOnScreen;
            CappedScreenPosition.x = Mathf.Clamp(CappedScreenPosition.x, _rectTransform.sizeDelta.x, Screen.width - _rectTransform.sizeDelta.x);
            CappedScreenPosition.y = Mathf.Clamp(CappedScreenPosition.y, _rectTransform.sizeDelta.y, Screen.height - _rectTransform.sizeDelta.y);

            _rectTransform.position = CappedScreenPosition;
        }
        else
        {
            _rectTransform.position = DestinationOnScreen;
        }
    }
}
