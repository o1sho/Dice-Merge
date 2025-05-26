using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private Player player;

    private void OnEnable() {
        if (player != null) {
            player.OnCircleOver += uiController.ToggleFinishPanel;
        }
    }

    private void OnDisable() {
        if (player != null) {
            player.OnCircleOver -= uiController.ToggleFinishPanel;
        }
    }


}
