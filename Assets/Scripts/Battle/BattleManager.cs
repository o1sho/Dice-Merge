using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {
    [SerializeField] private GameObject[] cardPrefabs; // ������ �������� ���� � ��������� ������
    [SerializeField] private Transform[] cardSpawnPoints; // ����� ������ ����
    [SerializeField] private Enemy enemy; // ������ �� �����

    private IEnemy _enemy; // ��������� �����
    private ICard[] _cards; // ������ �������� ����

    private void Awake() {
        // ������������� �����
        _enemy = enemy;
        if (_enemy == null) {
            Debug.LogError("Enemy is not assigned in BattleManager!");
            return;
        }

        // �������� �������� ����
        if (cardPrefabs == null || cardPrefabs.Length == 0) {
            Debug.LogError("Card prefabs array is empty!");
            return;
        }

        // �������� ����� ������
        if (cardSpawnPoints == null || cardSpawnPoints.Length == 0) {
            Debug.LogError("Card spawn points array is empty!");
            return;
        }

        // �������� �� null � ������� RectTransform
        for (int i = 0; i < cardSpawnPoints.Length; i++) {
            if (cardSpawnPoints[i] == null) {
                Debug.LogError($"Card spawn point at index {i} is missing!");
                return;
            }
            if (cardSpawnPoints[i].GetComponent<RectTransform>() == null) {
                Debug.LogError($"Card spawn point at index {i} does not have a RectTransform!");
                return;
            }
        }
    }

    private void Start() {
        // ��������� ���
        StartBattle();
    }

    private void StartBattle() {
        // ���������� �����
        _enemy.Reset();
        // ������� �����
        SpawnCards();
        Debug.Log("Battle started");
    }

    private void SpawnCards() {
        _cards = new ICard[cardSpawnPoints.Length];
        for (int i = 0; i < cardSpawnPoints.Length; i++) {
            // ����������, ���� ����� ������ �����������
            if (cardSpawnPoints[i] == null) {
                continue;
            }

            // �������� RectTransform ����� ������
            RectTransform spawnPointRect = cardSpawnPoints[i].GetComponent<RectTransform>();
            if (spawnPointRect == null) {
                continue;
            }

            // �������� ��������� ������ �� �������
            GameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Length)];
            // ������� �����
            GameObject cardObj = Instantiate(cardPrefab, spawnPointRect.position, Quaternion.identity, transform.parent);
            Card card = cardObj.GetComponent<Card>();
            if (card == null) {
                Debug.LogError($"Card component missing on prefab at index {i}");
                continue;
            }

            // ������������� �������
            RectTransform rect = cardObj.GetComponent<RectTransform>();
            rect.anchoredPosition = spawnPointRect.anchoredPosition;
            rect.localScale = Vector3.one;

            _cards[i] = card;
        }
    }

    public void MergeCards(ICard card1, ICard card2) {
        // ��������� ���������� �����
        if (card1.Type == card2.Type) {
            // ���������� ������
            card1.ActivateEffect(_enemy);
            // ���������� �����
            Destroy(card1.RectTransform.gameObject);
            Destroy(card2.RectTransform.gameObject);
            Debug.Log($"Merged {card1.Type} cards!");
        }
        else {
            // ���������� ������ ����� �� �������� �������
            card1.SetPosition(((Card)card1).OriginalPosition);
            Debug.Log($"Cannot merge {card1.Type} with {card2.Type}, returning to original position");
        }
    }

    public void EndBattle() {
        // ������������ � �������� �����
        SceneManager.LoadScene("HikeScene");
        Debug.Log("Battle ended");
    }
}