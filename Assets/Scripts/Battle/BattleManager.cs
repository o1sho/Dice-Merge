using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour {
    [SerializeField] private GameObject[] cardPrefabs; // Массив префабов карт с заданными типами
    [SerializeField] private Transform[] cardSpawnPoints; // Точки спавна карт
    [SerializeField] private Enemy enemy; // Ссылка на врага

    [SerializeField] private GameObject reloadCardPrefab;
    [SerializeField] private float reloadDelay = 2f;

    private IEnemy _enemy; // Интерфейс врага
    private ICard[] _cards; // Массив активных карт

    public ICard[] Cards => _cards;

    private void Awake() {
        // Инициализация врага
        _enemy = enemy;
        if (_enemy == null) {
            Debug.LogError("Enemy is not assigned in BattleManager!");
            return;
        }

        // Проверка префабов карт
        if (cardPrefabs == null || cardPrefabs.Length == 0) {
            Debug.LogError("Card prefabs array is empty!");
            return;
        }

        // Проверка точек спавна
        if (cardSpawnPoints == null || cardSpawnPoints.Length == 0) {
            Debug.LogError("Card spawn points array is empty!");
            return;
        }

        // Проверка на null и наличие RectTransform
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
        // Запускаем бой
        StartBattle();
    }

    private void StartBattle() {
        // Сбрасываем врага
        _enemy.Reset();
        // Спавним карты
        SpawnCards();
        Debug.Log("Battle started");
    }

    private void SpawnCards() {
        _cards = new ICard[cardSpawnPoints.Length];
        for (int i = 0; i < cardSpawnPoints.Length; i++) {
            // Пропускаем, если точка спавна отсутствует
            if (cardSpawnPoints[i] == null) {
                continue;
            }

            // Получаем RectTransform точки спавна
            RectTransform spawnPointRect = cardSpawnPoints[i].GetComponent<RectTransform>();
            if (spawnPointRect == null) {
                continue;
            }

            // Выбираем случайный префаб из массива
            GameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Length)];
            // Спавним карту
            GameObject cardObj = Instantiate(cardPrefab, spawnPointRect.position, Quaternion.identity, transform.parent);
            Card card = cardObj.GetComponent<Card>();
            if (card == null) {
                Debug.LogError($"Card component missing on prefab at index {i}");
                continue;
            }

            // Устанавливаем позицию
            RectTransform rect = cardObj.GetComponent<RectTransform>();
            rect.anchoredPosition = spawnPointRect.anchoredPosition;
            rect.localScale = Vector3.one;

            // Устанавливаем индекс слота
            card.SlotIndex = i;
            _cards[i] = card;
        }
    }

    public void MergeCards(ICard card1, ICard card2) {
        // Проверяем совпадение типов
        if (card1.Type == card2.Type) {
            // Активируем эффект
            card1.ActivateEffect(_enemy);

            // Получаем индексы карты

            int slotIndex1 = ((Card)card1).SlotIndex;
            int slotIndex2 = ((Card)card2).SlotIndex;

            // Уничтожаем карты
            Destroy(card1.RectTransform.gameObject);
            Destroy(card2.RectTransform.gameObject);
            Debug.Log($"Merged {card1.Type} cards!");

            SpawnReloadCard(slotIndex1);
            SpawnReloadCard(slotIndex2);
        }
        else {
            // Возвращаем первую карту на исходную позицию
            card1.SetPosition(((Card)card1).OriginalPosition);
            Debug.Log($"Cannot merge {card1.Type} with {card2.Type}, returning to original position");
        }
    }

    public void EndBattle() {
        // Возвращаемся в основную сцену
        SceneManager.LoadScene("HikeScene");
        Debug.Log("Battle ended");
    }

    private void SpawnReloadCard(int slotIndex) {
        // Спавним пустую карту
        GameObject reloadCardObj = Instantiate(reloadCardPrefab, transform.parent);
        RectTransform rect = reloadCardObj.GetComponent<RectTransform>();
        rect.anchoredPosition = cardSpawnPoints[slotIndex].GetComponent<RectTransform>().anchoredPosition;
        rect.localScale = Vector3.one;

        // Запускаем корутину перезарядки
        StartCoroutine(ReplaceEmptyCard(reloadCardObj, slotIndex));
    }

    private IEnumerator ReplaceEmptyCard(GameObject emptyCard, int slotIndex) {
        // Ждём заданное время
        yield return new WaitForSeconds(reloadDelay);

        // Получаем позицию пустой карты
        RectTransform emptyRect = emptyCard.GetComponent<RectTransform>();
        Vector2 position = emptyRect.anchoredPosition;

        // Уничтожаем пустую карту
        Destroy(emptyCard);

        // Спавним случайную карту
        GameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Length)];
        GameObject cardObj = Instantiate(cardPrefab, position, Quaternion.identity, transform.parent);
        Card card = cardObj.GetComponent<Card>();
        RectTransform cardRect = cardObj.GetComponent<RectTransform>();
        cardRect.anchoredPosition = cardSpawnPoints[slotIndex].GetComponent<RectTransform>().anchoredPosition;
        cardRect.localScale = Vector3.one;

        // Устанавливаем индекс слота и обновляем массив
        card.SlotIndex = slotIndex;
        _cards[slotIndex] = card;
    }
}