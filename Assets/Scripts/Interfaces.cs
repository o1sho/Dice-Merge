//���������� ��� ������� ����������, ����� ������� �������� �� ����������, � �� �� ���������� ����������.

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

// ���������� ��� Merge-��� //

public enum CardType { Attack, Defense, Magic }
public interface ICard {
    CardType Type { get; }
    RectTransform RectTransform { get; }
    void ActivateEffect(IPlayerFighter player, IEnemy enemy);
    void SetPosition(Vector2 position);
}

public interface IEnemy {
    int Health { get; }
    void TakeDamage(int damage);
    void DealDamage(int damage);
    void Reset();
}

public interface IPlayerFighter {
    int Health { get; }
    void TakeDamage(int damage);
    void Reset();
    void ActivateShield();
}