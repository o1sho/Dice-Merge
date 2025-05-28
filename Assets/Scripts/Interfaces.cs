//Интерфейсы для каждого компонента, чтобы скрипты зависели от абстракций, а не от конкретных реализаций.

using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public interface IPlayer {
    void MoveToTile(Vector2[] path, int targetTileIndex, float jumpDelay = 0.4f);
    int GetCurrentTileIndex();
    event Action OnLastTileReached;
    event Action<TileType> OnCardCollected;
    event Action OnMovementCompleted;
}

public interface IGameBoard {
    IReadOnlyList<Transform> Tiles { get; }
    void GenerateBoard();
    void ResetBoard();
    Vector2[] GetPath(int currentIndex, int diceRoll, out int newTileIndex);
}

public interface IDice {
    event Action<int> OnDiceRolled;
    event Action OnRollCompleted;
    void RollDice();
}

public interface IInventory {
    void AddCard(TileType type);
    string GetInventorySummary();
    void ClearInventory();
}

public interface IUIController {
    void ShowMenu();
    void HideMenu();
    void EnableRollDiceButton();
    event Action OnFightSelected;
    event Action OnContinueSelected;
    event Action OnRollDiceSelected;
}