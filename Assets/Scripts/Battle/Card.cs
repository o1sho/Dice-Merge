using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, ICard, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] private CardType type; // ��� ����� ����� ���������
    private RectTransform _rectTransform; // UI-���������
    private Vector2 _originalPosition; // �������� �������
    private Vector3 _originalScale; // �������� ������� �����

    public CardType Type => type;
    public RectTransform RectTransform => _rectTransform;

    private void Awake() {
        // �������� ����������
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = _rectTransform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // ���������� �������� �������
        _originalPosition = _rectTransform.anchoredPosition;
        // ����������� �����
        _rectTransform.localScale = _originalScale * 1.2f;
        // ��������� ���� ������
        _rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        // ���������� �����
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );
        _rectTransform.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // ���������� �������
        _rectTransform.localScale = _originalScale;

        // ���� �����, � ������� ������������
        Card targetCard = FindOverlappingCard();
        if (targetCard != null) {
            BattleManager battleManager = Object.FindFirstObjectByType<BattleManager>();
            if (battleManager != null) {
                battleManager.MergeCards(this, targetCard);
            }
            else {
                Debug.LogWarning("BattleManager not found in scene!");
                // ���������� �� �����, ���� �� ������� ����������
                _rectTransform.anchoredPosition = _originalPosition;
            }
        }
        else {
            // ���������� �� �����, ���� �� ����� �����������
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }

    private Card FindOverlappingCard() {
        // �������� ��� ����� � �����
        Card[] allCards = Object.FindObjectsByType<Card>(FindObjectsSortMode.None);
        foreach (Card otherCard in allCards) {
            if (otherCard != this) {
                // ��������� ����������� ����� RectTransform
                Rect thisRect = GetWorldRect(_rectTransform);
                Rect otherRect = GetWorldRect(otherCard._rectTransform);
                if (thisRect.Overlaps(otherRect)) {
                    Debug.Log($"Card {type} overlaps with {otherCard.type}");
                    return otherCard;
                }
            }
        }
        return null;
    }

    private Rect GetWorldRect(RectTransform rectTransform) {
        // �������� ���� RectTransform � ������� �����������
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // ������ Rect �� �����
        Vector2 min = corners[0];
        Vector2 max = corners[2];
        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    public void ActivateEffect(IEnemy enemy) {
        // ������ �� ����
        switch (type) {
            case CardType.Attack:
                if (enemy != null) {
                    enemy.TakeDamage(10);
                    Debug.Log("Attack effect: 10 damage dealt!");
                }
                break;
            case CardType.Defense:
                Debug.Log("Defense effect activated");
                break;
            case CardType.Magic:
                Debug.Log("Magic effect activated");
                break;
        }
    }

    public void SetPosition(Vector2 position) {
        // ������������� �������
        _rectTransform.anchoredPosition = position;
    }

    public Vector2 OriginalPosition => _originalPosition;
}