using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Sword _sword;

    public static ActiveWeapon Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Player.Instance.IsAlive)
            FollowMousePosition();
    }

    public Sword GetActiveWeapon()
    {
        return _sword;
    }

    private void FollowMousePosition()
    {
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerPosition();

        if (mousePosition.x < playerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}