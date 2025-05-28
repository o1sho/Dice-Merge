using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour, IInventory {
    private readonly Dictionary<TileType, int> inventory = new Dictionary<TileType, int>
    {
        { TileType.Attack, 0 },
        { TileType.Defense, 0 },
        { TileType.Ability, 0 }
    };

    public void AddCard(TileType type) {
        if (type != TileType.Start) {
            inventory[type]++;
            Debug.Log($"Added {type} card. Total: {inventory[type]}");
        }
    }

    public string GetInventorySummary() {
        return $"Attack: {inventory[TileType.Attack]}, Defense: {inventory[TileType.Defense]}, Ability: {inventory[TileType.Ability]}";
    }

    public void ClearInventory() {
        inventory[TileType.Attack] = 0;
        inventory[TileType.Defense] = 0;
        inventory[TileType.Ability] = 0;
        Debug.Log("Inventory cleared.");
    }
}