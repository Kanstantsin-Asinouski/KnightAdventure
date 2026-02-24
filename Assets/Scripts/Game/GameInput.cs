using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game
{
    public class GameInput : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        public static GameInput Instance { get; private set; }

        public event EventHandler OnPlayerAttack;

        private void Awake()
        {
            Instance = this;

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Enable();

            _playerInputActions.Combat.Attack.started += Player_AttackStarted;
        }

        private void OnDestroy()
        {
            _playerInputActions.Combat.Attack.started -= Player_AttackStarted;
            _playerInputActions.Disable();
        }

        public Vector2 GetMovementVector()
        {
            return _playerInputActions.Player.Move.ReadValue<Vector2>();
        }

        public Vector2 GetMousePosition()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            return mousePos;
        }

        public void DisableMovement()
        {
            _playerInputActions.Disable();
        }

        private void Player_AttackStarted(InputAction.CallbackContext obj)
        {
            OnPlayerAttack?.Invoke(this, EventArgs.Empty);
        }
    }
}