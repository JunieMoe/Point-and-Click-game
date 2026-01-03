using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    public GameObject widgetPrefab;
    public Transform spawnPosition;

    public void SpawnWidget()
    {
        Instantiate(widgetPrefab, spawnPosition.position, Quaternion.identity);
    }
}