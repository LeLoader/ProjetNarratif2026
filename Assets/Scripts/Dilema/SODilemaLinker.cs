using EditorAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SODilemaLinker", menuName = "ScriptableObjects/SODilemaLinker")]
public class SODilemaLinker : ScriptableObject, INotifyBindablePropertyChanged
{
    [SerializeField] SODilema currentDilema;

    [SerializeField] Button Choice1Button;
    [SerializeField] Button Choice2Button;
    [SerializeField] TMP_Text QuestionLabel;

    public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

    public void SetDilema(SODilema dilema)
    {
        currentDilema = dilema;
        QuestionLabel.text = currentDilema.question.GetLocalizedString();
        Choice1Button.text = currentDilema.firstChoice.label.GetLocalizedString();
        Choice2Button.text = currentDilema.secondChoice.label.GetLocalizedString();
        Choice1Button.clicked += dilema.firstChoice.Activate;
        Choice2Button.clicked += dilema.secondChoice.Activate;
    }
}
