using UnityEngine;

public enum TileType { Attack, Defense, Ability, Start }

public class Tile : MonoBehaviour {
    [SerializeField] private TileType tileType;
    public TileType GetTileType() => tileType;
}