using EditorAttributes;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class DilemaController : MonoBehaviour
{
    [SerializeField] SODilema TestDilema;
    SODilema currentDilema;

    [SerializeField] TMP_Text questionLabel;
    [SerializeField] Button firstChoiceButton;
    [SerializeField] TMP_Text firstChoiceLabel;
    [SerializeField] Button secondChoiceButton;
    [SerializeField] TMP_Text secondChoiceLabel;

    [Button]
    public void LoadTestDilema()
    {
        SetDilema(TestDilema);
    }

    public void LoadCurrentDilema()
    {
        questionLabel.text = currentDilema.question.GetLocalizedString();
        firstChoiceLabel.text = currentDilema.firstChoice.label.GetLocalizedString();
        secondChoiceLabel.text = currentDilema.secondChoice.label.GetLocalizedString();
        // firstChoiceButton.onClick += dilema.firstChoice.Activate();
        // secondChoiceButton.onClick += dilema.secondChoice.Activate;

        Debug.Log($"Loaded dilema: {currentDilema}");
    }

    public void SetDilema(SODilema dilema)
    {
        if (dilema)
        {
            currentDilema = dilema;
            LoadCurrentDilema();
        }
    }

    [Button]
    public void PlayRandomDilema()
    {
        SODilema dilema = DilemaManager.instance.GetCurrentDilema();
        SetDilema(dilema);
    }

    public void Awake()
    {
        LocalizationSettings.SelectedLocaleChanged += (locale) => { LoadCurrentDilema(); };
    }
}
