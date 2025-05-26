using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputSystem_Actions inputSystem;

    private void Awake() {
        Instance = this;

        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();
    }

}
