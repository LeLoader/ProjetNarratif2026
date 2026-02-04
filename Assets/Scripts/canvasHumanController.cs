using System;
using TMPro;
using UnityEngine;

public class CanvasHumanController : MonoBehaviour
{
    [SerializeField] private GameObject _textAboveHeadPrefab;
    
    public void ShowTextAboveHead(string text)
    {
        UpdateTowardCamera();
        
        textDialogueController textInstance = Instantiate(_textAboveHeadPrefab, transform).GetComponent<textDialogueController>();
        if (textInstance != null)
        {
            textInstance.Initialize(text);
        }
    }
    
    private void UpdateTowardCamera()
    {
        if (Camera.main != null)
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            directionToCamera.y = 0;
            if (directionToCamera != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
                transform.rotation = lookRotation;
            }
        }
    }
}
