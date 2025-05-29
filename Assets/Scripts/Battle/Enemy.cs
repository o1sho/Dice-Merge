using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy {
    [SerializeField] private int maxHealth = 50;

    private int _health;

    public int Health => _health;

    private void Awake() {
        _health = maxHealth;
    }

    public void TakeDamage(int damage) {
        _health -= damage;
        Debug.Log($"Enemy took {damage} damage, health: {_health}");
        if (_health <= 0) {
            FindFirstObjectByType<BattleManager>().EndBattle();
        }
    }

    public void Reset() {
        _health = maxHealth;
        Debug.Log("Enemy reset");
    }
}