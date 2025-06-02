using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.2f; // Длительность дрожания
    [SerializeField] private float shakeMagnitude = 0.1f; // Амплитуда дрожания

    private Vector3 _originalPosition;
    private bool _isShaking;

    private void Awake() {
        _originalPosition = transform.localPosition;
    }

    public void ShakeCamera() {
        if (!_isShaking) {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake() {
        _isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration) {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = _originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalPosition;
        _isShaking = false;
    }
}
