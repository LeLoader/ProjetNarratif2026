using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [Header("UI SLIDER")]
    
    [SerializeField] private Slider _stateProgressSlider;
    
    [Header("DILEMMA")]
    
    [SerializeField] private GameObject _dilemmaPanel;
    
    [SerializeField] private TextMeshProUGUI dilemmaTextUI;
    
    [SerializeField] private TextMeshProUGUI _choice1;
    [SerializeField] private TextMeshProUGUI _choice2;
    
    [SerializeField] private Button _choice1Button;
    [SerializeField] private Button _choice2Button;

    public event Action OnDilemmaChosen;

    #region DILEMMA METHODS
    
    
    public void ShowDilemma(SODilema dilema)
    {
        _dilemmaPanel.SetActive(true);
        
        // INIT UI //
        
        dilemmaTextUI.text = dilema.question.GetLocalizedString();
        _choice1.text = dilema.firstChoice.label.GetLocalizedString();
        _choice2.text = dilema.secondChoice.label.GetLocalizedString();
        
        // INIT BUTTONS //
        
        _choice1Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.firstChoice));
        _choice2Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.secondChoice));
    }

    private void ChoseAnswer(SODilema dilemma, Choice choice)
    {
        OnDilemmaChosen.Invoke();
        dilemma.Choose(choice);
        
        _dilemmaPanel.SetActive(false);
        
        _choice1Button.onClick.RemoveAllListeners();
        _choice2Button.onClick.RemoveAllListeners();
    }
    
    #endregion

    #region Slider

    
    public void UpdateStateProgressSlider(int value)
    {
        _stateProgressSlider.value = value;
    }

    #endregion

    
}
