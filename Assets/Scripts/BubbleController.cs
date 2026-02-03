using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleController : MonoBehaviour, IPointerClickHandler
{
    public event Action OnClicked;

    [SerializeField] private RectTransform _rectTransform;

    private Vector3 _destination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 DestinationOnScreen = Camera.main.WorldToScreenPoint(_destination);
        bool IsOffScreen = DestinationOnScreen.x < 0 || DestinationOnScreen.x > Screen.width || DestinationOnScreen.y < 0 || DestinationOnScreen.y > Screen.height;
        if (IsOffScreen)
        {
            Vector3 CappedScreenPosition = DestinationOnScreen;
            if (CappedScreenPosition.x <= 0) { CappedScreenPosition.x = 0; }
            if (CappedScreenPosition.x >= Screen.width) { CappedScreenPosition.x = Screen.width; }
            if (CappedScreenPosition.y <= 0) { CappedScreenPosition.y = 0; }
            if (CappedScreenPosition.y >= Screen.height) { CappedScreenPosition.y = Screen.height; }

            Vector3 PointerWorldPosition = Camera.main.ScreenToWorldPoint(CappedScreenPosition);
            PointerWorldPosition.z = 0;
            _rectTransform.position = PointerWorldPosition;
        }
    }

    public void SetDestination(Vector3 Destination)
    {
        _destination = Destination;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[BUBBLE CONTROLLER] bubble clicked");
        OnClicked?.Invoke();
    }
}
