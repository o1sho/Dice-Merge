//������ ����� �������������� �������������� ����� ������������.

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject player; // ������ �� GameObject � IPlayer
    [SerializeField] private GameObject gameBoard; // ������ �� GameObject � IGameBoard
    [SerializeField] private GameObject dice; // ������ �� GameObject � IDice
    [SerializeField] private GameObject uiController; // ������ �� GameObject � IUIController
    [SerializeField] private GameObject inventory; // ������ �� GameObject � IInventory

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

        if (_player == null || _gameBoard == null || _dice == null || _uiController == null || _inventory == null) {
            Debug.LogError("One or more dependencies are missing in GameManager!");
        }
    }

    private void OnEnable() {
        _dice.OnDiceRolled += HandleDiceRoll;

        _player.OnLastTileReached += HandleLastTileReached;
        _player.OnCardCollected += _inventory.AddCard;
        _player.OnMovementCompleted += HandleMovementCompleted;

        _uiController.OnContinueSelected += HandleContinue;
        _uiController.OnFightSelected += HandleFight;
        _uiController.OnRollDiceSelected += HandleRollDiceButton;
    }

    private void OnDisable() {
        _dice.OnDiceRolled -= HandleDiceRoll;

        _player.OnLastTileReached -= HandleLastTileReached;
        _player.OnCardCollected -= _inventory.AddCard;
        _player.OnMovementCompleted -= HandleMovementCompleted;

        _uiController.OnContinueSelected -= HandleContinue;
        _uiController.OnFightSelected -= HandleFight;
        _uiController.OnRollDiceSelected -= HandleRollDiceButton;
    }

    private void Start() {
        _gameBoard.GenerateBoard();
        _uiController.HideMenu();
        _isPaused = false;
        player.GetComponent<Player>().SetTilesCount(_gameBoard.Tiles.Count);
    }

    private void HandleRollDiceButton() {
        if (!_isPaused) {
            _dice.RollDice();
        }
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
        if (!_isPaused) return;
        _uiController.HideMenu();
        _isPaused = false;
        Time.timeScale = 1f;
        _gameBoard.ResetBoard();
        _player.MoveToTile(new Vector2[] { _gameBoard.Tiles[0].position }, 0);
        //_player.StopMovement();
    }

    private void HandleFight() {
        _uiController.HideMenu();
        _isPaused = false;
        Time.timeScale = 1f;
        _inventory.ClearInventory();
        SceneManager.LoadScene("BattleScene");
        Debug.Log("Fight started!");
    }

    private void HandleMovementCompleted() {
        if (!_isPaused) // �������� ������ ������ ���� �� �� �����
        {
            _uiController.EnableRollDiceButton();
        }
    }
}