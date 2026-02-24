using UnityEngine;

namespace Assets.Scripts.Weapons.Sword
{
    [RequireComponent(typeof(Animator))]
    public class SwordSlashVisual : MonoBehaviour
    {
        [SerializeField] private Sword sword;

        private Animator _animator;

        private static readonly int AttackHash = Animator.StringToHash(Attack);

        private const string Attack = "Attack";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            sword.OnSwordSwing += Sword_OnSwordSwing;
        }

        private void OnDestroy()
        {
            sword.OnSwordSwing -= Sword_OnSwordSwing;
        }

        private void Sword_OnSwordSwing(object sender, System.EventArgs e)
        {
            _animator.SetTrigger(AttackHash);
        }
    }
}