using Assets.Scripts.Game;
using Assets.Scripts.Weapons.Sword;
using System;
using UnityEngine;

namespace Assets.Scripts.Plants
{
    public class DestructiblePlant : MonoBehaviour
    {
        public event EventHandler OnDesctrictibleTakenDamage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Sword>())
            {
                OnDesctrictibleTakenDamage?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);

                NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
            }
        }
    }
}