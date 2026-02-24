using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KnockBack : MonoBehaviour
    {
        [SerializeField] private float knockBackForce;
        [SerializeField] private float knockBackMovingTimeMax;

        private Rigidbody2D _rb;

        private float _knockBackMovingTimeTimer;

        public bool IsGettingKnockBack { get; private set; }

        private void Start()
        {
            knockBackForce = 3f;
            knockBackMovingTimeMax = 0.3f;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!IsGettingKnockBack)
                return;

            _knockBackMovingTimeTimer -= Time.deltaTime;

            if (_knockBackMovingTimeTimer < 0)
            {
                StopKnockBackMovement();
            }
        }

        public void GetKnockedBack(Transform damageSource)
        {
            IsGettingKnockBack = true;
            _knockBackMovingTimeTimer = knockBackMovingTimeMax;
            Vector2 difference = (transform.position - damageSource.position).normalized * knockBackForce;
            _rb.AddForce(difference, ForceMode2D.Impulse);
        }

        public void StopKnockBackMovement()
        {
            _rb.linearVelocity = Vector2.zero;
            IsGettingKnockBack = false;
        }
    }
}