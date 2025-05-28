using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {
    [Header("Tile Prefabs")]
    [SerializeField] private GameObject startTilePrefab;
    [SerializeField] private GameObject bossTilePrefab;

    [SerializeField] private GameObject simpleTilePrefab;
    [SerializeField] private GameObject attackTilePrefab;
    [SerializeField] private GameObject defenseTilePrefab;
    [SerializeField] private GameObject abilityTilePrefab;

    [Header("Tile Positions")]
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform bossPos;
    [SerializeField] private List<Transform> tilePositions = new List<Transform>();
    public List<Transform> TilePositions => tilePositions;

    [Header("Path Settings")]

    [Header("Line")]
    [SerializeField] private LineRenderer lineRenderer;

    private List<GameObject> spawnedTiles = new List<GameObject>();

    private void Start() {
        GeneratePath();
    }

    private void GeneratePath() {
        spawnedTiles.Clear();

        // Спавн стартового тайла
        GameObject start = Instantiate(startTilePrefab, startPos.position, Quaternion.identity);
        spawnedTiles.Add(start);

        // Спавн босса
        GameObject boss = Instantiate(bossTilePrefab, bossPos.position, Quaternion.identity);
        spawnedTiles.Add(boss);

        foreach (Transform pos in tilePositions) {
            GameObject tile = Instantiate(GetRandomTile(), pos.position, Quaternion.identity);
            spawnedTiles.Add(tile);
        }



        //DrawPathLines();
    }

    private GameObject GetRandomTile() {
        GameObject[] tileOptions = { simpleTilePrefab, attackTilePrefab, defenseTilePrefab, abilityTilePrefab };
        GameObject tile = tileOptions[Random.Range(0, tileOptions.Length)];
        return tile;
    }





    //private void DrawPathLines() {
    //    if (lineRenderer == null || spawnedTiles.Count == 0) return;

    //    lineRenderer.positionCount = spawnedTiles.Count;

    //    for (int i = 0; i < spawnedTiles.Count; i++) {
    //        lineRenderer.SetPosition(i, spawnedTiles[i].transform.position + Vector3.up * 0.1f);
    //    }
    //}
}
