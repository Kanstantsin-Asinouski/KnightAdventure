using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAi _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private const string _IS_RUNNING = "IsRunning";
    private const string _IS_DIE = "IsDie";
    private const string _TAKE_HIT = "TakeHit";
    private const string _CHASING_SPEED_MULTUPLIER = "ChasingSpeedMultiplier";
    private const string _ATTACK = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    private void Update()
    {
        _animator.SetBool(_IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(_CHASING_SPEED_MULTUPLIER, _enemyAI.GetRoamingAnimationSpeed);
    }
    
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.AttackColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.AttackColliderTurnOn();
    }

    private void _enemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(_ATTACK); 
    }
    private void _enemyEntity_OnTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(_TAKE_HIT);
    }

    private void _enemyEntity_OnDeath(object sender, EventArgs e)
    {
        _animator.SetBool(_IS_DIE, true);
        _spriteRenderer.sortingLayerID = 0;
        _enemyShadow.SetActive(false);
    }
}