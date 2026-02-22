using UnityEngine;

public class DestructiblePlantVisual : MonoBehaviour
{
    [SerializeField] private DestructiblePlant _destructiblePlant;
    [SerializeField] private GameObject _bushDeathVFXPrefab;

    private void Start()
    {
        _destructiblePlant.OnDesctrictibleTakenDamage += DesctrictiblePlant_OnDestructibleTakenDamage;
    }

    private void OnDestroy()
    {
        _destructiblePlant.OnDesctrictibleTakenDamage -= DesctrictiblePlant_OnDestructibleTakenDamage;
    }

    private void DesctrictiblePlant_OnDestructibleTakenDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(_bushDeathVFXPrefab, transform.transform.position, Quaternion.identity);
    }
}