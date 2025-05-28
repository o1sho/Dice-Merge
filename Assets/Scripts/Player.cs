//Отвечает только за движение и взаимодействие с плитками.

using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, IPlayer {
    [SerializeField] private int tilesCount;
    private Tile currentTile;
    private int currentTileIndex = 0;

    public event Action OnLastTileReached;
    public event Action<TileType> OnCardCollected;
    public event Action OnMovementCompleted;

    private void OnTriggerEnter2D(Collider2D other) {
        // Проверяем триггер последней точки спавна
        if (other.CompareTag("LastTilePoint")) {
            OnLastTileReached?.Invoke();
            Debug.Log("Player reached last tile point via trigger");
        }

        // Обрабатываем плитки
        Tile tile = other.GetComponent<Tile>();
        if (tile != null) {
            currentTile = tile;
        }
    }

    public void MoveToTile(Vector2[] path, int targetTileIndex, float jumpDelay = 0.2f) {
        StartCoroutine(MoveCoroutine(path, targetTileIndex, jumpDelay));
    }

    public int GetCurrentTileIndex() => currentTileIndex;

    public void StopMovement() {
        StopAllCoroutines();
        Debug.Log("Player movement stopped");
    }

    public void SetTilesCount(int count) => tilesCount = count;

    private IEnumerator MoveCoroutine(Vector2[] path, int targetTileIndex, float jumpDelay) {

        int pathIndex = currentTileIndex;

        foreach (Vector2 position in path) {
            pathIndex = (pathIndex + 1) % tilesCount;
            transform.position = position;
            yield return new WaitForSeconds(jumpDelay);
        }


        currentTileIndex = targetTileIndex;

        if (currentTile != null) {
            OnCardCollected?.Invoke(currentTile.GetTileType());
        }

        OnMovementCompleted?.Invoke();
    }
}