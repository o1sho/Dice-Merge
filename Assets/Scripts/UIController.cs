//Только управляет UI, не вызывает методы других скриптов напрямую.

using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour, IUIController {
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button fightButton;
    [SerializeField] private Button continueButton;

    public event Action OnFightSelected;
    public event Action OnContinueSelected;

    private void Start() {
        menuPanel.SetActive(false);
        fightButton.onClick.AddListener(() => OnFightSelected?.Invoke());
        continueButton.onClick.AddListener(() => OnContinueSelected?.Invoke());
    }

    public void ShowMenu() => menuPanel.SetActive(true);
    public void HideMenu() => menuPanel.SetActive(false);
}
