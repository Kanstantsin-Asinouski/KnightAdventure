using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyEntity : MonoBehaviour
    {
        [SerializeField] private EnemySO enemySO;
        private int _currentHealth;

        private PolygonCollider2D _polygonCollider2D;
        private BoxCollider2D _boxCollider2D;
        private EnemyAi _enemyAi;

        public event EventHandler OnTakeHit;
        public event EventHandler OnDeath;

        private void Awake()
        {
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _enemyAi = GetComponent<EnemyAi>();

            _polygonCollider2D.enabled = false;
        }

        private void Start()
        {
            _currentHealth = enemySO.enemyHealth;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_polygonCollider2D.enabled)
                return;

            if (collision.transform.TryGetComponent(out Player.Player player))
            {
                player.TakeDamage(transform, enemySO.enemyDamageAmount);
            }
        }

        public void AttackColliderTurnOff()
        {
            _polygonCollider2D.enabled = false;
        }

        public void AttackColliderTurnOn()
        {
            _polygonCollider2D.enabled = true;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            DetectDeath();
        }

        private void DetectDeath()
        {
            if (_currentHealth <= 0)
            {
                _boxCollider2D.enabled = false;
                _polygonCollider2D.enabled = false;

                _enemyAi.SetDeathState();

                OnDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}