using UnityEngine;
using UnityEngine.AI;
using Friends.Utils;
using System;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 2f;
    private float _nextAttackTime = 0f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextCheckDirectionTime = 0f;
    private float _nextDirectionDuration = 0.1f;

    public event EventHandler OnEnemyAttack;

    public bool IsRunning => _navMeshAgent.velocity.sqrMagnitude > 0.0001f;

    public float GetRoamingAnimationSpeed
    {
        get => _navMeshAgent.speed / _roamingSpeed;
    }

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _currentState = _startingState;

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;

        _navMeshAgent.stoppingDistance = _attackingDistance * 0.9f;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:

                _roamingTimer -= Time.deltaTime;

                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }

                CheckCurrentState();

                break;
            case State.Chasing:

                ChasingTarget();
                CheckCurrentState();

                break;
            case State.Attacking:

                AttackingTarget();
                CheckCurrentState();

                break;
            case State.Death:
                SetDeathState();
                break;
        }
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }
            else
            {
                var v = _navMeshAgent.desiredVelocity;
                if (v.sqrMagnitude > 0.0001f)
                {
                    if (v.x < 0)
                        transform.rotation = Quaternion.Euler(0, -180, 0);
                    else
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }

            _nextCheckDirectionTime = Time.time + _nextDirectionDuration;
        }
    }


    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        State newState = State.Roaming;

        if (_isChasingEnemy)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                if (Player.Instance.IsAlive)
                {
                    newState = State.Attacking;
                }
                else
                {
                    newState = State.Roaming;
                }
            }
        }

        if (_isAttackingEnemy)
        {
            if (distanceToPlayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _navMeshAgent.isStopped = false;
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
                _navMeshAgent.velocity = Vector3.zero;
            }

            _currentState = newState;
        }

    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + FriendsUtils.GetRandomDir() * Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}