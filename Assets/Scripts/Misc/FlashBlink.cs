using System;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Keep the shared material asset as the default so we can reliably restore it later.
        _defaultMaterial = _spriteRenderer.sharedMaterial;

        // Start not blinking; blinking will be triggered by the event.
        _isBlinking = false;

        if (_damagableObject is Player player)
        {
            player.OnFlashBlink += DamagableObject_OnFlashBlink;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks / invalid callbacks
        if (_damagableObject is Player player)
        {
            player.OnFlashBlink -= DamagableObject_OnFlashBlink;
        }
    }

    private void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer <= 0f)
            {
                RestoreDefaultMaterial();
            }
        }
    }

    // Call to immediately stop blinking and restore the default material.
    public void StopBlinking()
    {
        _isBlinking = false;
        RestoreDefaultMaterial();
    }

    private void StartBlinkingMaterial()
    {
        // Use the blink material for the duration. Make this an instance to avoid
        // accidental shared-material modifications elsewhere.
        _spriteRenderer.material = _blinkMaterial;
        _blinkTimer = _blinkDuration;
        _isBlinking = true;
    }

    private void RestoreDefaultMaterial()
    {
        // Restore the shared default material asset so other systems (like death material)
        // that set sharedMaterial are not unintentionally overwritten by an instance.
        _spriteRenderer.sharedMaterial = _defaultMaterial;
        _isBlinking = false;
    }

    private void DamagableObject_OnFlashBlink(object sender, EventArgs e)
    {
        // If a death or other system has already changed the material (for example,
        // to a death material), avoid overriding it. We only start blink if the renderer
        // currently uses the default material.
        if (_spriteRenderer.sharedMaterial == _defaultMaterial)
        {
            StartBlinkingMaterial();
        }
    }
}