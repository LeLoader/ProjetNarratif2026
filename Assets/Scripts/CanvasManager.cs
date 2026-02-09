using ChristinaCreatesGames.Typography.Typewriter;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
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

    [SerializeField] private TextMeshProUGUI questionTextUIStatic;
    [SerializeField] private TextMeshProUGUI questionTextUI;
    [SerializeField] private TextMeshProUGUI longAnswerTextUI;

    [SerializeField] private TextMeshProUGUI _choice1;
    [SerializeField] private TextMeshProUGUI _choice2;

    [SerializeField] private Button _choice1Button;
    [SerializeField] private Button _choice2Button;

    [Header("INPUTS")]
    [SerializeField] private InputActionReference skipInput;

    public event Action OnDilemmaEnded;

    #region DILEMMA METHODS


    public void ShowDilemma(SODilemma dilema, BehaviorController controller)
    {
        _dilemmaPanel.SetActive(true);

        // INIT UI //

        questionTextUI.text = dilema.question.GetLocalizedString();
        longAnswerTextUI.text = "";
        questionTextUIStatic.text = "";
        _choice1.text = dilema.firstChoice.shortAnswerLabel.GetLocalizedString();
        _choice2.text = dilema.secondChoice.shortAnswerLabel.GetLocalizedString();

        // INIT BUTTONS //

        _choice1Button.gameObject.SetActive(false);
        _choice2Button.gameObject.SetActive(false);
        _choice1Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.firstChoice, controller));
        _choice2Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.secondChoice, controller));

        // 

        TypewriterEffect effect = questionTextUI.GetComponent<TypewriterEffect>();
        effect.CompleteTextRevealed += () => 
        {
            Timer timer = gameObject.AddComponent<Timer>();
            timer.Internal_Start(1, true);
            timer.OnTimerElapsed += () =>
            {
                questionTextUIStatic.text = dilema.question.GetLocalizedString();
                questionTextUI.text = "";
                _choice1Button.gameObject.SetActive(true);
                _choice2Button.gameObject.SetActive(true);
            };
            questionTextUI.GetComponent<Animation>().Play();
        };
    }

    private void ChoseAnswer(SODilemma dilemma, Choice choice, BehaviorController controller)
    {
        longAnswerTextUI.text = choice.longAnswerLabel.GetLocalizedString();
        TypewriterEffect effect = longAnswerTextUI.GetComponent<TypewriterEffect>();
        if (effect)
        {
            Action onCompletedTextRevealed = () => { };
            onCompletedTextRevealed = () => {
                Timer timer = gameObject.AddComponent<Timer>();
                timer.Internal_Start(1, true);
                timer.OnTimerElapsed += () =>
                {
                    Action<InputAction.CallbackContext> a = (InputAction.CallbackContext ctx) => { };

                    a = (InputAction.CallbackContext ctx) =>
                    {
                        dilemma.Choose(choice, controller);
                        OnDilemmaEnded.Invoke();
                        _dilemmaPanel.SetActive(false);
                        skipInput.action.started -= a;
                    };

                    skipInput.action.started += a;
                };
                effect.CompleteTextRevealed -= onCompletedTextRevealed;
            };

            effect.CompleteTextRevealed += onCompletedTextRevealed;
        }
        else
        {
            _dilemmaPanel.SetActive(false);
            dilemma.Choose(choice, controller);
        }

        _choice1Button.onClick.RemoveAllListeners();
        _choice1Button.gameObject.SetActive(false);
        _choice2Button.onClick.RemoveAllListeners();
        _choice2Button.gameObject.SetActive(false);
    }

    #endregion

    #region Slider


    public void UpdateStateProgressSlider(int value)
    {
        _stateProgressSlider.value = value;
    }

    #endregion


}