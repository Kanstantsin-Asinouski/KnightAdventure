using Assets.Scripts.Game;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class ActiveWeapon : MonoBehaviour
    {
        [SerializeField] private Sword.Sword sword;

        public static ActiveWeapon Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Player.Player.Instance.IsAlive)
                FollowMousePosition();
        }

        public Sword.Sword GetActiveWeapon()
        {
            return sword;
        }

        private void FollowMousePosition()
        {
            Vector3 mousePosition = GameInput.Instance.GetMousePosition();
            Vector3 playerPosition = Player.Player.Instance.GetPlayerPosition();

            transform.rotation = Quaternion.Euler(0, mousePosition.x < playerPosition.x ? 180 : 0, 0);
        }
    }
}