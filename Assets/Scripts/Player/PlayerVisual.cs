using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private FlashBlink _flashBlink;

    private static readonly int DieHash = Animator.StringToHash(IsDie);
    private static readonly int RunningHash = Animator.StringToHash(IsRunning);

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

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(DieHash, true);
        _flashBlink.StopBlinking();
    }

    private void Update()
    {
        _animator.SetBool(RunningHash, Player.Instance.IsRunning());

        if (Player.Instance.IsAlive)
            AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerPosition();

        _spriteRenderer.flipX = mousePosition.x < playerPosition.x;
    }
}