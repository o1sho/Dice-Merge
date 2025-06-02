using UnityEngine;

public class PlayerFighter : MonoBehaviour, IPlayerFighter {

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float shieldDuration = 3f;

    private int _health;
    private bool _isShielded;
    private float _shieldTimer;

    public int Health => _health;

    private void Awake() {
        _health = maxHealth;
    }

    private void Update() {
        if (_isShielded) {
            _shieldTimer -= Time.deltaTime;
            if (_shieldTimer <= 0) {
                _isShielded = false;
            }
        }
    }

    public void TakeDamage(int damage) {
        if (_isShielded) {
            Debug.Log("Player is shielded, no damage taken!");
            return;
        }

        _health -= damage;
        Debug.Log($"Player took {damage} damage, health: {_health}");

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null) {
            cameraShake.ShakeCamera();
        }

        if (_health <= 0) {
            Debug.Log("Player defeated!");
            GetComponentInParent<BattleManager>().EndBattle();
        }
    }

    public void Reset() {
        _health = maxHealth;
        _isShielded = false;
        _shieldTimer = 0f;
    }

    public void ActivateShield() {
        _isShielded = true;
        _shieldTimer = shieldDuration;
        Debug.Log("Player is now shielded!");
    }
}
