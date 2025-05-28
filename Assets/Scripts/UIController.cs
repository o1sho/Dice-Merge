//������ ��������� UI, �� �������� ������ ������ �������� ��������.

using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour, IUIController {
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button fightButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button rollDiceButton;

    public event Action OnFightSelected;
    public event Action OnContinueSelected;
    public event Action OnRollDiceSelected;

    private void Start() {
        menuPanel.SetActive(false);
        fightButton.onClick.AddListener(() => OnFightSelected?.Invoke());
        continueButton.onClick.AddListener(() => OnContinueSelected?.Invoke());
        rollDiceButton.onClick.AddListener(() => {
            OnRollDiceSelected?.Invoke();
            rollDiceButton.interactable = false; // ��������� ������
            Debug.Log("RollDice button disabled");
        });
    }

    public void ShowMenu() {
        menuPanel.SetActive(true);
        rollDiceButton.interactable = false; // ��������� ������ ��� ������ ����
        Debug.Log("Menu shown, RollDice button disabled");
    }

    public void HideMenu() {
        menuPanel.SetActive(false);
        rollDiceButton.interactable = true; // �������� ������ ��� ������� ����
        Debug.Log("Menu hidden, RollDice button enabled");
    }

    public void EnableRollDiceButton() {
        rollDiceButton.interactable = true;
        Debug.Log("RollDice button enabled after movement");
    }
}
