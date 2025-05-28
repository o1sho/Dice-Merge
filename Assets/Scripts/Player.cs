//�������� ������ �� �������� � �������������� � ��������.

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

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // ��������� ������� ��������� ����� ������
        if (other.CompareTag("LastTilePoint")) {
            OnLastTileReached?.Invoke();
            Debug.Log("Player reached last tile point via trigger");
        }

        // ������������ ������
        Tile tile = other.GetComponent<Tile>();
        if (tile != null) {
            currentTile = tile;
        }
    }

    public void MoveToTile(Vector2[] path, int targetTileIndex, float jumpDelay = 0.4f) {
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

        Vector2 previousPosition = transform.position;


        //animator.SetBool("isMoveFront", true);

        foreach (Vector2 position in path) {
            pathIndex = (pathIndex + 1) % tilesCount;

            float deltaX = position.x - previousPosition.x;
            float deltaY = position.y - previousPosition.y;

            animator.SetBool("isMoveFront", false);
            animator.SetBool("isMoveBack", false);
            animator.SetBool("isMoveSide", false);

            // ������������� �������� � ����������� �� �����������
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) // �������������� ��������
            {
                if (deltaX < 0) // �������� �����
                {
                    animator.SetBool("isMoveSide", true);
                    spriteRenderer.flipX = false;
                }
                else if (deltaX > 0) {  // �������� ������
                    
                    animator.SetBool("isMoveSide", true);
                    spriteRenderer.flipX = true;
                }
            }
            else // ������������ ��������
            {
                if (deltaY > 0) // �������� �����
                {
                    animator.SetBool("isMoveBack", true);
                }
                else if (deltaY < 0) // �������� ����
                {
                    animator.SetBool("isMoveFront", true);
                }
            }

            transform.position = position;
            previousPosition = position; // ��������� ���������� �������
            yield return new WaitForSeconds(jumpDelay);
        }


        currentTileIndex = targetTileIndex;
        // ���������� ��� ��������� ����� ���������� ��������
        animator.SetBool("isMoveFront", false);
        animator.SetBool("isMoveBack", false);
        animator.SetBool("isMoveSide", false);

        if (currentTile != null) {
            OnCardCollected?.Invoke(currentTile.GetTileType());
        }

        OnMovementCompleted?.Invoke();
    }
}