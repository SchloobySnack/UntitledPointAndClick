using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // The prefab for the enemy
    public GameObject enemyPrefab;

    // Called when the spawner should spawn an enemy
    public void SpawnEnemy()
    {
        // Instantiate the enemy prefab at the spawner's position and rotation
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}