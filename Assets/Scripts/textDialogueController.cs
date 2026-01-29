using TMPro;
using UnityEngine;

public class textDialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    
    
    
    
    public void Initialize(string text)
    {
        if (_textMeshPro != null)
        {
            _textMeshPro.SetText(text);
        }
    }
    
}
