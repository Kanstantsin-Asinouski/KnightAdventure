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
            _animator.SetBool(_IS_RUNNING, enemyAI.IsRunning);
            _animator.SetFloat(_CHASING_SPEED_MULTUPLIER, enemyAI.GetRoamingAnimationSpeed);
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
            enemyShadow.SetActive(false);
        }
    }
}