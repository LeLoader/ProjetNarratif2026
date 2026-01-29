using System;
using TMPro;
using UnityEngine;

public class canvasHumanController : MonoBehaviour
{
    [SerializeField] private GameObject _textAboveHeadPrefab;
    
    public void ShowTextAboveHead(string text)
    {
        UpdateTowardCamera();
        
        TextMeshPro textInstance = Instantiate(_textAboveHeadPrefab, transform).GetComponent<TextMeshPro>();
        if (textInstance != null)
        {
            textInstance.SetText(text);
        }
    }
    
    private void UpdateTowardCamera()
    {
        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
