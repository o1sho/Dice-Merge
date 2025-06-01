using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour {
    [SerializeField] private GameObject[] cardPrefabs; // ������ �������� ���� � ��������� ������
    [SerializeField] private Transform[] cardSpawnPoints; // ����� ������ ����
    [SerializeField] private Enemy enemy; // ������ �� �����

    [SerializeField] private GameObject reloadCardPrefab;
    [SerializeField] private float reloadDelay = 2f;

    private IEnemy _enemy; // ��������� �����
    private ICard[] _cards; // ������ �������� ����

    public ICard[] Cards => _cards;

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

            // ������������� ������ �����
            card.SlotIndex = i;
            _cards[i] = card;
        }
    }

    public void MergeCards(ICard card1, ICard card2) {
        // ��������� ���������� �����
        if (card1.Type == card2.Type) {
            // ���������� ������
            card1.ActivateEffect(_enemy);

            // �������� ������� �����

            int slotIndex1 = ((Card)card1).SlotIndex;
            int slotIndex2 = ((Card)card2).SlotIndex;

            // ���������� �����
            Destroy(card1.RectTransform.gameObject);
            Destroy(card2.RectTransform.gameObject);
            Debug.Log($"Merged {card1.Type} cards!");

            SpawnReloadCard(slotIndex1);
            SpawnReloadCard(slotIndex2);
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

    private void SpawnReloadCard(int slotIndex) {
        // ������� ������ �����
        GameObject reloadCardObj = Instantiate(reloadCardPrefab, transform.parent);
        RectTransform rect = reloadCardObj.GetComponent<RectTransform>();
        rect.anchoredPosition = cardSpawnPoints[slotIndex].GetComponent<RectTransform>().anchoredPosition;
        rect.localScale = Vector3.one;

        // ��������� �������� �����������
        StartCoroutine(ReplaceEmptyCard(reloadCardObj, slotIndex));
    }

    private IEnumerator ReplaceEmptyCard(GameObject emptyCard, int slotIndex) {
        // ��� �������� �����
        yield return new WaitForSeconds(reloadDelay);

        // �������� ������� ������ �����
        RectTransform emptyRect = emptyCard.GetComponent<RectTransform>();
        Vector2 position = emptyRect.anchoredPosition;

        // ���������� ������ �����
        Destroy(emptyCard);

        // ������� ��������� �����
        GameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Length)];
        GameObject cardObj = Instantiate(cardPrefab, position, Quaternion.identity, transform.parent);
        Card card = cardObj.GetComponent<Card>();
        RectTransform cardRect = cardObj.GetComponent<RectTransform>();
        cardRect.anchoredPosition = cardSpawnPoints[slotIndex].GetComponent<RectTransform>().anchoredPosition;
        cardRect.localScale = Vector3.one;

        // ������������� ������ ����� � ��������� ������
        card.SlotIndex = slotIndex;
        _cards[slotIndex] = card;
    }
}