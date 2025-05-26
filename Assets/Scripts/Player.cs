using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private TileSpawner tileSpawner;

    [SerializeField] private float moveDurationTime = 0.5f;

    private int currentTileIndex = 0;
    private bool isMoving = false;

    private void OnEnable() {
        if (dice != null) {
            dice.OnDiceRolled += HandleDiceResult;
        }
    }

    private void OnDisable() {
        if (dice != null) {
            dice.OnDiceRolled -= HandleDiceResult;
        }
    }

    private void Start() {
        transform.position = new Vector2(0, -4);
    }

    private void HandleDiceResult(int result) {
        if (!isMoving) {
            StartCoroutine(MovePlayer(result));
        }
    }

    private IEnumerator MovePlayer(int steps) {
        isMoving = true;

        for (int i = 0; i < steps; i++) {
            int targetIndex = currentTileIndex + 1;

            if (targetIndex - 1 >= tileSpawner.TilePositions.Count) {
                Debug.LogWarning("Игрок достиг последней позиции!");
                break;
            }

            Vector3 startPos = transform.position;
            Vector3 targetPos = tileSpawner.TilePositions[targetIndex - 1].position;

            float elapsedTime = 0;

            // Плавное перемещение между точками
            while (elapsedTime < moveDurationTime) {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDurationTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
            currentTileIndex = targetIndex;

        }

        isMoving = false;
    }
}
