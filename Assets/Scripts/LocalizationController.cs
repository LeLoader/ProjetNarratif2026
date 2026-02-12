using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationController : MonoBehaviour
{
    [SerializeField] TMP_Text pressToStartText;
    [SerializeField] LocalizedString pressToStartString;
    [SerializeField] TMP_Text changeLocaleText;
    [SerializeField] LocalizedString changeLocaleString;

    [SerializeField] List<Locale> allLocales;
    private int currentIndex = -1;

    private void Start()
    {
        allLocales = LocalizationSettings.AvailableLocales.Locales;

        LocalizationSettings.SelectedLocaleChanged += (Locale locale) =>
        {
            pressToStartText.text = pressToStartString.GetLocalizedString();
            changeLocaleText.text = changeLocaleString.GetLocalizedString();
        };

        ChangeLocale();
    }

    public void ChangeLocale()
    {
        if (++currentIndex >= allLocales.Count)
        {
            currentIndex = 0;
        }

        LocalizationSettings.SelectedLocale = allLocales[currentIndex];
    }
}
