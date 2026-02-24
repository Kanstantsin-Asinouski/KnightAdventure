using Assets.Scripts.Game;
using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerVisual : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private FlashBlink _flashBlink;

        private const string IsRunning = "IsRunning";
        private const string IsDie = "IsDie";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _flashBlink = GetComponent<FlashBlink>();
        }

        private void Start()
        {
            Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        }

        private void OnDestroy()
        {
            Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
        }

        private void Player_OnPlayerDeath(object sender, System.EventArgs e)
        {
            _animator.SetBool(IsDie, true);
            _flashBlink.StopBlinking();
        }

        private void Update()
        {
            _animator.SetBool(IsRunning, Player.Instance.IsRunning());

            if (Player.Instance.IsAlive)
                AdjustPlayerFacingDirection();
        }

        private void AdjustPlayerFacingDirection()
        {
            Vector3 mousePosition = GameInput.Instance.GetMousePosition();
            Vector3 playerPosition = Player.Instance.GetPlayerPosition();

            if (mousePosition.x < playerPosition.x)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
}