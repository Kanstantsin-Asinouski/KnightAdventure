using Assets.Scripts.Game;
using Assets.Scripts.Weapons;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        public event EventHandler OnPlayerDeath;
        public event EventHandler OnFlashBlink;

        [SerializeField] private float movingSpeed = 5f;
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float damageRecoveryTime = 0.5f;

        private Rigidbody2D _rigidBody;
        private KnockBack _knockBack;

        private readonly float _minMovingSpeed = 0.1f;
        private bool _canTakeDamage;
        private bool _isAlive;
        private int _currentHealth;
        private bool _isRunning;
        private Vector2 _inputVector;

        private Camera _mainCamera;

        private void Awake()
        {
            Instance = this;
            _rigidBody = GetComponent<Rigidbody2D>();
            _knockBack = GetComponent<KnockBack>();

            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _currentHealth = maxHealth;
            _canTakeDamage = true;
            _isAlive = true;
            _isRunning = false;
            GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        }

        private void OnDestroy()
        {
            GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        }

        public void Update()
        {
            _inputVector = GameInput.Instance.GetMovementVector();
        }

        private void FixedUpdate()
        {
            if (_knockBack.IsGettingKnockBack)
            {
                return;
            }

            HandleMovement();
        }

        public bool IsAlive => _isAlive;

        public void TakeDamage(Transform damageSource, int damage)
        {
            if (_canTakeDamage && _isAlive)
            {
                _canTakeDamage = false;
                _currentHealth = Mathf.Max(0, _currentHealth -= damage);

                _knockBack.GetKnockedBack(damageSource);

                OnFlashBlink?.Invoke(this, EventArgs.Empty);

                StartCoroutine(DamageRecoveryRoutine());
            }

            DetectDamage();
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public Vector3 GetPlayerPosition()
        {
            Vector3 playerPosition = _mainCamera.WorldToScreenPoint(transform.position);
            return playerPosition;
        }

        private void GameInput_OnPlayerAttack(object sender, EventArgs e)
        {
            ActiveWeapon.Instance.GetActiveWeapon().Attack();
        }

        private void HandleMovement()
        {
            _rigidBody.MovePosition(_rigidBody.position + _inputVector * (movingSpeed * Time.fixedDeltaTime));

            if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed)
            {
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
            }
        }

        private IEnumerator DamageRecoveryRoutine()
        {
            yield return new WaitForSeconds(damageRecoveryTime);
            _canTakeDamage = true;
        }

        private void DetectDamage()
        {
            if (_currentHealth == 0 && _isAlive)
            {
                _isAlive = false;
                _knockBack.StopKnockBackMovement();
                GameInput.Instance.DisableMovement();

                OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}