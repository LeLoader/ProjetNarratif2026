using ChristinaCreatesGames.Typography.Typewriter;
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
    [SerializeField] private TextMeshProUGUI longAnswerTextUI;

    [SerializeField] private TextMeshProUGUI _choice1;
    [SerializeField] private TextMeshProUGUI _choice2;

    [SerializeField] private Button _choice1Button;
    [SerializeField] private Button _choice2Button;

    public event Action OnDilemmaEnded;

    #region DILEMMA METHODS


    public void ShowDilemma(SODilemma dilema)
    {
        _dilemmaPanel.SetActive(true);

        // INIT UI //

        dilemmaTextUI.text = dilema.question.GetLocalizedString();
        longAnswerTextUI.text = "";
        _choice1.text = dilema.firstChoice.shortAnswerLabel.GetLocalizedString();
        _choice2.text = dilema.secondChoice.shortAnswerLabel.GetLocalizedString();

        // INIT BUTTONS //

        _choice1Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.firstChoice));
        _choice2Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.secondChoice));
    }

    private void ChoseAnswer(SODilemma dilemma, Choice choice)
    {
        longAnswerTextUI.text = choice.longAnswerLabel.GetLocalizedString();
        TypewriterEffect effect = longAnswerTextUI.GetComponent<TypewriterEffect>();
        if (effect)
        {
            effect.CompleteTextRevealed += () =>
            {
                OnDilemmaEnded.Invoke();
                _dilemmaPanel.SetActive(false);
                dilemma.Choose(choice);
            };
        }
        else
        {
            _dilemmaPanel.SetActive(false);
            dilemma.Choose(choice);
        }

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
