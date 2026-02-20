using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Rigidbody2D _rigidBody;
    private KnockBack _knockBack;

    private readonly float _minMovingSpeed = 0.1f;
    private bool _canTakeDamage;
    private int _currentHealth;
    private bool _isRunning;
    private Vector2 _inputVector;

    private void Awake()
    {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        _isRunning = false;
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
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

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            Debug.Log(_currentHealth);
            _knockBack.GetKnockedBack(damageSource);

            StartCoroutine(DamageRecoveryRoutine());
        }
    }



    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerPosition;
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void HandleMovement()
    {
        _rigidBody.MovePosition(_rigidBody.position + _inputVector * (_movingSpeed * Time.fixedDeltaTime));

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
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }
}