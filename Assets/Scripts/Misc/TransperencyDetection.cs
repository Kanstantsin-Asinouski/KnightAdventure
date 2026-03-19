using System.Collections;
using UnityEngine;

public class TransperencyDetection : MonoBehaviour
{
    private const float FULL_NON_TRANSPARENT = 1.0f;

    [Range(0f, 1f)]
    [SerializeField] private float trasperencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.5f;

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.attachedRigidbody != null
            ? collider.attachedRigidbody.GetComponent<Player>()
            : null;

        if (player != null)
        {
            if (collider is CapsuleCollider2D)
            {
                StartCoroutine(FadeRoutine(
                    _spriteRenderer,
                    fadeTime,
                    _spriteRenderer.color.a,
                    trasperencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Player player = collider.attachedRigidbody != null
            ? collider.attachedRigidbody.GetComponent<Player>()
            : null;

        if (player != null)
        {
            if (collider is CapsuleCollider2D)
            {
                StartCoroutine(FadeRoutine(
                    _spriteRenderer,
                    fadeTime,
                    _spriteRenderer.color.a,
                    FULL_NON_TRANSPARENT));
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime,
        float startTransperencyAmount, float targetTransperencyAmount)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTransperencyAmount, targetTransperencyAmount, elapsedTime / fadeTime);

            _spriteRenderer.color = new Color(
                _spriteRenderer.color.r,
                _spriteRenderer.color.g,
                _spriteRenderer.color.b,
                newAlpha);
            yield return null;
        }
    }
}