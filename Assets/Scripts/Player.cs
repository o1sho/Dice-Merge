//Отвечает только за движение и взаимодействие с плитками.

using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, IPlayer {
    [SerializeField] private int tilesCount; // Кол-во плиток, передаётся через инспектор или GameManager
    private Tile currentTile;
    private bool isMoving = false;
    private bool isInitialPlacement = true;
    private int currentTileIndex = 0;

    public event Action OnLastTileReached;
    public event Action<TileType> OnCardCollected;

    private void OnTriggerEnter2D(Collider2D other) {
        Tile tile = other.GetComponent<Tile>();
        if (tile != null) {
            currentTile = tile;
        }
    }

    public void MoveToTile(Vector2[] path, int targetTileIndex, float jumpDelay = 0.2f) {
        StartCoroutine(MoveCoroutine(path, targetTileIndex, jumpDelay));
    }

    public int GetCurrentTileIndex() => currentTileIndex;

    private IEnumerator MoveCoroutine(Vector2[] path, int targetTileIndex, float jumpDelay) {
        isMoving = true;
        int pathIndex = currentTileIndex;

        foreach (Vector2 position in path) {
            pathIndex = (pathIndex + 1) % tilesCount;
            transform.position = position;

            if (pathIndex == tilesCount - 1) {
                OnLastTileReached?.Invoke();
            }

            yield return new WaitForSeconds(jumpDelay);
        }

        isMoving = false;
        isInitialPlacement = false;
        currentTileIndex = targetTileIndex;

        if (currentTile != null) {
            OnCardCollected?.Invoke(currentTile.GetTileType());
        }
    }
}