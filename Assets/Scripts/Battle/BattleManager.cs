using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {
    [SerializeField] private GameObject[] cardPrefabs; // Массив префабов карт с заданными типами
    [SerializeField] private Transform[] cardSpawnPoints; // Точки спавна карт
    [SerializeField] private Enemy enemy; // Ссылка на врага

    private IEnemy _enemy; // Интерфейс врага
    private ICard[] _cards; // Массив активных карт

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

            _cards[i] = card;
        }
    }

    public void MergeCards(ICard card1, ICard card2) {
        // Проверяем совпадение типов
        if (card1.Type == card2.Type) {
            // Активируем эффект
            card1.ActivateEffect(_enemy);
            // Уничтожаем карты
            Destroy(card1.RectTransform.gameObject);
            Destroy(card2.RectTransform.gameObject);
            Debug.Log($"Merged {card1.Type} cards!");
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
}