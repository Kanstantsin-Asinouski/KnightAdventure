using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAi enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private static readonly int DieHash = Animator.StringToHash(IsDie);
    private static readonly int TakeHitHash = Animator.StringToHash(TakeHit);
    private static readonly int RunningHash = Animator.StringToHash(IsRunning);
    private static readonly int SpeedMultiplierHash = Animator.StringToHash(ChasingSpeedMultiplier);
    private static readonly int AttackHash = Animator.StringToHash(Attack);

    private const string IsRunning = "IsRunning";
    private const string IsDie = "IsDie";
    private const string TakeHit = "TakeHit";
    private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";
    private const string Attack = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += EnemyEntity_OnTakeHit;
        enemyEntity.OnDeath += EnemyEntity_OnDeath;
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= EnemyAI_OnEnemyAttack;
    }

    private void Update()
    {
        _animator.SetBool(RunningHash, enemyAI.IsRunning);
        _animator.SetFloat(SpeedMultiplierHash, enemyAI.GetRoamingAnimationSpeed);
    }
    
    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.AttackColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.AttackColliderTurnOn();
    }

    private void EnemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(AttackHash); 
    }
    private void EnemyEntity_OnTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TakeHitHash);
    }

    private void EnemyEntity_OnDeath(object sender, EventArgs e)
    {
        _animator.SetBool(DieHash, true);
        _spriteRenderer.sortingLayerID = 0;
        enemyShadow.SetActive(false);
    }
}