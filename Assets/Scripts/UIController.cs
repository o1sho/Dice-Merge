using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject finishPanel;

    private void Start() {
        finishPanel.SetActive(false);
    }

    public void ToggleFinishPanel() {
        finishPanel.SetActive(!finishPanel.activeSelf);
    }

    public void OnContinueButton() {
        ToggleFinishPanel();
    }
}
