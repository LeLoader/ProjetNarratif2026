using ChristinaCreatesGames.Typography.Typewriter;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [SerializeField] private InputManager _inputManager;

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
    [SerializeField] private TextMeshProUGUI skipTextUI;
    [SerializeField] private TextMeshProUGUI longAnswerTextUI;
    [SerializeField] private Image _questionBackground;

    [SerializeField] private TextMeshProUGUI _choice1;
    [SerializeField] private TextMeshProUGUI _choice2;

    [SerializeField] private Button _choice1Button;
    [SerializeField] private Button _choice2Button;

    [SerializeField] private LocalizedString skipString;

    [Header("End")]
    [SerializeField] private Image EndImage;
    [SerializeField] private Sprite EndOne;
    [SerializeField] private Sprite EndFree;

    [Header("INPUTS")]
    [SerializeField] private InputActionReference skipInput;

    public event Action OnDilemmaEnded;

    #region DILEMMA METHODS


    public void ShowDilemma(SODilemma dilema, BehaviorController controller)
    {
        _inputManager.EnteringDilemma();
        // INIT UI //

        questionTextUI.text = dilema.question.GetLocalizedString();
        longAnswerTextUI.text = "";
        questionTextUIStatic.text = "";
        _choice1.text = dilema.firstChoice.shortAnswerLabel.GetLocalizedString();
        _choice2.text = dilema.secondChoice.shortAnswerLabel.GetLocalizedString();
        _questionBackground.enabled = true;
        skipTextUI.text = "";

        // INIT BUTTONS //

        _choice1Button.gameObject.SetActive(false);
        _choice2Button.gameObject.SetActive(false);
        _choice1Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.firstChoice, controller));
        _choice2Button.onClick.AddListener(() => ChoseAnswer(dilema, dilema.secondChoice, controller));

        // 

        TypewriterEffect effect = questionTextUI.GetComponent<TypewriterEffect>();
        Action OnCompleteTextRevealed = () => { };
        OnCompleteTextRevealed = () =>
        {
            Timer.SetTimer(gameObject, 1.1f, true).OnTimerElapsed += () =>
            {
                effect.CompleteTextRevealed -= OnCompleteTextRevealed;
                questionTextUIStatic.text = $"{controller.name}: {dilema.question.GetLocalizedString()}";
                questionTextUI.text = "";
                _choice1Button.gameObject.SetActive(true);
                _choice2Button.gameObject.SetActive(true);
                _questionBackground.enabled = false;
            };
            questionTextUI.GetComponent<Animation>().Play();
        };
        effect.CompleteTextRevealed += OnCompleteTextRevealed;

        _dilemmaPanel.SetActive(true);
    }

    private void ChoseAnswer(SODilemma dilemma, Choice choice, BehaviorController controller)
    {
        longAnswerTextUI.text = choice.longAnswerLabel.GetLocalizedString();
        TypewriterEffect effect = longAnswerTextUI.GetComponent<TypewriterEffect>();
        
        if (effect)
        {
            Action onCompletedTextRevealed = () => { };
            onCompletedTextRevealed = () =>
            {
                TypewriterEffect effectSkip = skipTextUI.GetComponent<TypewriterEffect>();
                Action onSkipTextRevealed = () => { };
                onSkipTextRevealed = () =>
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
                    effectSkip.CompleteTextRevealed -= onSkipTextRevealed;
                };
                _questionBackground.enabled = false;
                skipTextUI.text = skipString.GetLocalizedString();
                effectSkip.CompleteTextRevealed += onSkipTextRevealed;


                effect.CompleteTextRevealed -= onCompletedTextRevealed;
            };
            effect.CompleteTextRevealed += onCompletedTextRevealed;
        }
        else
        {
            dilemma.Choose(choice, controller);
            OnDilemmaEnded.Invoke();
            _dilemmaPanel.SetActive(false);
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

    public void FadeToBlack(bool IsOne)
    {
        StartCoroutine(FadeToBlackCoroutine(IsOne));
    }

    private IEnumerator FadeToBlackCoroutine(bool IsOne)
    {
        float alpha = 0;
        while (alpha <= 1)
        {
            alpha += Time.deltaTime * 0.05f;
            Color color = EndImage.color;
            color.a = alpha;
            EndImage.color = color;
            yield return null;
        }
        EndImage.color = Color.white;
        if (IsOne)
        {
            EndImage.sprite = EndOne;
        } else
        {
            EndImage.sprite = EndFree;
        }
    }

    public void AllowMovement()
    {
        _inputManager.ExitingDilemma();
    }

}