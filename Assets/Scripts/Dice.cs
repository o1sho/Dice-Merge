using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    public event Action<int> OnDiceRolled;

    public void GetRandomValue() {
        int result = UnityEngine.Random.Range(1, 7);
        OnDiceRolled?.Invoke(result);
        Debug.Log(result);
    }

    public void OnClickRollDiceButton() {
        GetRandomValue();
    }


}
