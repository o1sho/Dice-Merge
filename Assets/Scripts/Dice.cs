//Только генерирует бросок и уведомляет через событие.

using UnityEngine;
using System;

public class Dice : MonoBehaviour, IDice {
    public event Action<int> OnDiceRolled;

    public void RollDice() {
        int roll = UnityEngine.Random.Range(1, 7);
        Debug.Log($"Dice rolled: {roll}");
        OnDiceRolled?.Invoke(roll);
    }
}
