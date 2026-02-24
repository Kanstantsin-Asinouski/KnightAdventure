using UnityEngine;

namespace Assets.Scripts.Plants
{
    public class DestructiblePlantVisual : MonoBehaviour
    {
        [SerializeField] private DestructiblePlant destructiblePlant;
        [SerializeField] private GameObject bushDeathVFXPrefab;

        private void Start()
        {
            destructiblePlant.OnDesctrictibleTakenDamage += DesctrictiblePlant_OnDestructibleTakenDamage;
        }

        private void OnDestroy()
        {
            destructiblePlant.OnDesctrictibleTakenDamage -= DesctrictiblePlant_OnDestructibleTakenDamage;
        }

        private void DesctrictiblePlant_OnDestructibleTakenDamage(object sender, System.EventArgs e)
        {
            ShowDeathVFX();
        }

        private void ShowDeathVFX()
        {
            Instantiate(bushDeathVFXPrefab, transform.transform.position, Quaternion.identity);
        }
    }
}