//Отвечает только за генерацию и сброс поля, предоставление пути для движения.

using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour, IGameBoard {
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject startTilePrefab;
    [SerializeField] private Transform[] tilePoints;

    private readonly List<Transform> tiles = new List<Transform>();
    public IReadOnlyList<Transform> Tiles => tiles.AsReadOnly();

    public void GenerateBoard() {
        if (tilePoints == null || tilePoints.Length == 0) {
            Debug.LogError("TilePoints array is empty!");
            return;
        }

        tiles.Clear();
        for (int i = 0; i < tilePoints.Length; i++) {
            GameObject tilePrefab = tilePoints[i].name == "TilePoint_1_start" ? startTilePrefab : tilePrefabs[Random.Range(0, tilePrefabs.Length)];
            GameObject tile = Instantiate(tilePrefab, tilePoints[i].position, Quaternion.identity, transform);
            tiles.Add(tile.transform);
        }
    }

    public void ResetBoard() {
        foreach (Transform tile in tiles) {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
        GenerateBoard();
    }

    public Vector2[] GetPath(int currentIndex, int diceRoll, out int newTileIndex) {
        newTileIndex = (currentIndex + diceRoll) % tiles.Count;
        List<Vector2> path = new List<Vector2>();
        int index = currentIndex;

        for (int i = 0; i < diceRoll; i++) {
            index = (index + 1) % tiles.Count;
            path.Add(tiles[index].position);
        }

        return path.ToArray();
    }
}