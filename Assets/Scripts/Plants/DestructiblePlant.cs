using System;
using UnityEngine;

public class DestructiblePlant : MonoBehaviour
{
    public event EventHandler OnDesctrictibleTakenDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sword>())
        {
            OnDesctrictibleTakenDamage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);

            NavMeshSurfacManagement.Instance.RebakeNavmeshSurface();
        }
    }
}