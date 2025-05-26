using UnityEngine;

public class Dice : MonoBehaviour
{
    public event System.Action<int> OnDiceRolled;

    public void GetRandomValue() {
        int result = Random.Range(1, 7);
        OnDiceRolled?.Invoke(result);
        Debug.Log(result);
    }

    public void OnClickRollDiceButton() {
        GetRandomValue();
    }


}
