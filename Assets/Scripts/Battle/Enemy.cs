using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IEnemy {
    [SerializeField] private int maxHealth = 50;

    [SerializeField] private int attackDamage = 10; // ���� �����
    [SerializeField] private float attackInterval = 5f; // �������� ����� �������

    private Animator _animator; // ��� ��������
    private IPlayerFighter _player; // ������ �� ������
    private bool _isAttacking;

    private int _health;

    public int Health => _health;

    private void Awake() {
        _health = maxHealth;
        _animator = GetComponent<Animator>();
        if (_animator == null) {
            Debug.LogError("Animator component missing on Enemy!");
        }
    }

    private void Start() {
        // ������� ������ ����� BattleManager
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        _player = battleManager.Player;

        StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack() {
        while (true) {
            yield return new WaitForSeconds(attackInterval);

            _animator.SetTrigger("isPreparedForAttack");

            Debug.Log("Enemy is preparing to attack!");

            
        }
    }

    public void DealDamageForAnimator() {
        // ��������� �����
            DealDamage(attackDamage);
    }

    public void TakeDamage(int damage) {
        _health -= damage;
        Debug.Log($"Enemy took {damage} damage, health: {_health}");
        if (_health <= 0) {
            FindFirstObjectByType<BattleManager>().EndBattle();
        }
    }

    public void DealDamage(int damage) {
        _player.TakeDamage(damage);
        Debug.Log($"Enemy deals {damage} damage to player!");
    }

    public void Reset() {
        _health = maxHealth;
        Debug.Log("Enemy reset");
    }
}