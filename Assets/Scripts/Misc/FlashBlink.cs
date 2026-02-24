using System;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class FlashBlink : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour damagableObject;
        [SerializeField] private Material blinkMaterial;
        [SerializeField] private float blinkDuration = 0.2f;

        private float _blinkTimer;
        private Material _defaultMaterial;
        private SpriteRenderer _spriteRenderer;
        private bool _isBlinking;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _defaultMaterial = _spriteRenderer.sharedMaterial;

            _isBlinking = false;
        }

        private void Start()
        {
            if (damagableObject is Player.Player player)
            {
                player.OnFlashBlink += DamagableObject_OnFlashBlink;
            }
        }

        private void OnDestroy()
        {
            if (damagableObject is Player.Player player)
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

        public void StopBlinking()
        {
            _isBlinking = false;
            RestoreDefaultMaterial();
        }

        private void StartBlinkingMaterial()
        {
            _spriteRenderer.material = blinkMaterial;
            _blinkTimer = blinkDuration;
            _isBlinking = true;
        }

        private void RestoreDefaultMaterial()
        {
            _spriteRenderer.sharedMaterial = _defaultMaterial;
            _isBlinking = false;
        }

        private void DamagableObject_OnFlashBlink(object sender, EventArgs e)
        {
            if (_spriteRenderer.sharedMaterial == _defaultMaterial)
            {
                StartBlinkingMaterial();
            }
        }
    }
}