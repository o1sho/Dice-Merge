using UnityEngine;
using System.Collections.Generic;

public class Data : MonoBehaviour {
    private Dictionary<TileType, int> inventory = new Dictionary<TileType, int>
    {
        { TileType.Attack, 0 },
        { TileType.Defense, 0 },
        { TileType.Ability, 0 }
    }; // ������� ���� �� �����

    public static Data Instance { get; private set; }
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ���������� ����� � ���������
    public void AddCard(TileType type) {
        if (type != TileType.Start) // ��������� ������ �� ��������� ����
        {
            inventory[type]++;
            Debug.Log($"Added {type} card. Total: {inventory[type]}");
        }
    }

    // ��������� �������� ���������
    public Dictionary<TileType, int> GetInventory() {
        return inventory;
    }

    // ��������� ���������� �������� ���������
    public string GetInventorySummary() {
        return $"Attack: {inventory[TileType.Attack]}, Defense: {inventory[TileType.Defense]}, Ability: {inventory[TileType.Ability]}";
    }

    // ������� ��������� (��������, ����� ���)
    public void ClearInventory() {
        inventory[TileType.Attack] = 0;
        inventory[TileType.Defense] = 0;
        inventory[TileType.Ability] = 0;
        Debug.Log("Inventory cleared.");
    }
}