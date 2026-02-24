using System;
using UnityEngine;

namespace Assets.Scripts.Enemies.Skeleton
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SkeletonVisual : MonoBehaviour
    {
        [SerializeField] private EnemyAi enemyAI;
        [SerializeField] private EnemyEntity enemyEntity;
        [SerializeField] private GameObject enemyShadow;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

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
            enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
            enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
            enemyEntity.OnDeath += _enemyEntity_OnDeath;
        }

        private void OnDestroy()
        {
            enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
            enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
            enemyEntity.OnDeath -= _enemyEntity_OnDeath;
        }

        private void Update()
        {
            _animator.SetBool(IsRunning, enemyAI.IsRunning);
            _animator.SetFloat(ChasingSpeedMultiplier, enemyAI.GetRoamingAnimationSpeed);
        }

        public void TriggerAttackAnimationTurnOff()
        {
            enemyEntity.AttackColliderTurnOff();
        }

        public void TriggerAttackAnimationTurnOn()
        {
            enemyEntity.AttackColliderTurnOn();
        }

        private void _enemyAI_OnEnemyAttack(object sender, EventArgs e)
        {
            _animator.SetTrigger(Attack);
        }
        private void _enemyEntity_OnTakeHit(object sender, EventArgs e)
        {
            _animator.SetTrigger(TakeHit);
        }

        private void _enemyEntity_OnDeath(object sender, EventArgs e)
        {
            _animator.SetBool(IsDie, true);
            _spriteRenderer.sortingLayerID = 0;
            enemyShadow.SetActive(false);
        }
    }
}