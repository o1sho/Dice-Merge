//—крипт будет координировать взаимодействие между компонентами.

using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject player; // —сылка на GameObject с IPlayer
    [SerializeField] private GameObject gameBoard; // —сылка на GameObject с IGameBoard
    [SerializeField] private GameObject dice; // —сылка на GameObject с IDice
    [SerializeField] private GameObject uiController; // —сылка на GameObject с IUIController
    [SerializeField] private GameObject inventory; // —сылка на GameObject с IInventory

    private IPlayer _player;
    private IGameBoard _gameBoard;
    private IDice _dice;
    private IUIController _uiController;
    private IInventory _inventory;
    private bool _isPaused;

    private void Awake() {
        _player = player.GetComponent<IPlayer>();
        _gameBoard = gameBoard.GetComponent<IGameBoard>();
        _dice = dice.GetComponent<IDice>();
        _uiController = uiController.GetComponent<IUIController>();
        _inventory = inventory.GetComponent<IInventory>();
    }

    private void OnEnable() {
        _dice.OnDiceRolled += HandleDiceRoll;
        _player.OnLastTileReached += HandleLastTileReached;
        _player.OnCardCollected += _inventory.AddCard;
        _uiController.OnContinueSelected += HandleContinue;
        _uiController.OnFightSelected += HandleFight;
    }

    private void OnDisable() {
        _dice.OnDiceRolled -= HandleDiceRoll;
        _player.OnLastTileReached -= HandleLastTileReached;
        _player.OnCardCollected -= _inventory.AddCard;
        _uiController.OnContinueSelected -= HandleContinue;
        _uiController.OnFightSelected -= HandleFight;
    }

    private void Start() {
        _gameBoard.GenerateBoard();
        _uiController.HideMenu();
        _isPaused = false;
    }

    private void HandleDiceRoll(int roll) {
        if (!_isPaused) {
            Vector2[] path = _gameBoard.GetPath(_player.GetCurrentTileIndex(), roll, out int newTileIndex);
            _player.MoveToTile(path, newTileIndex);
        }
    }

    private void HandleLastTileReached() {
        _isPaused = true;
        Time.timeScale = 0f;
        _uiController.ShowMenu();
    }

    private void HandleContinue() {
        _isPaused = false;
        Time.timeScale = 1f;
        _gameBoard.ResetBoard();
        _player.MoveToTile(new Vector2[] { _gameBoard.Tiles[0].position }, 0);
    }

    private void HandleFight() {
        _isPaused = false;
        Time.timeScale = 1f;
        _inventory.ClearInventory();
        Debug.Log("Fight started!");
        // «десь можно добавить логику бо€
    }
}