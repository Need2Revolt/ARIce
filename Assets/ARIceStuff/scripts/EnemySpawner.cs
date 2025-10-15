// 23/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EnemySpawner : MonoBehaviour
{
    private float startingDelay = 15f;
    private float delayDecrease = 1f;
    private float minDelay = 2f;

    public int maxEnemies = 10;

    public GameObject[] enemyPrefabs;

    private List<GameObject> enemiesList;

    private int enemyCount = 0;

    private float timer;

    public int[] direction = { 1, -1 };

    private int kills;

    public ARMeshManager meshManager; // Reference to the ARMeshManager for accessing the mesh data
    public Transform player; // Reference to the player's transform

    private float validationInterval = 10f; // Time interval for validation
    private float validationTimer = 0f;

    void Start()
    {
        timer = 0;
        enemiesList = new List<GameObject>();
        kills = 0;

        if (meshManager == null)
        {
            Debug.LogError("ARMeshManager is not assigned. Please assign it in the inspector.");
        }

        if (player == null)
        {
            Debug.LogError("Player transform is not assigned. Please assign it in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        validationTimer += Time.deltaTime;

        if (timer > startingDelay)
        {
            if (enemyCount < maxEnemies)
            {
                enemyCount++;
                SpawnEnemy();
                if (startingDelay > minDelay)
                {
                    startingDelay -= delayDecrease;
                    timer = 0;
                }
            }
        }

        // Periodically validate enemies
        if (validationTimer > validationInterval)
        {
            ValidateEnemies();
            validationTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        // Generate a random position around the player
        Vector3 randomPosition = new Vector3(
            Random.Range(0.3f, 1.5f) * direction[Random.Range(0, 2)],
            Random.Range(0.1f, 0.5f),
            Random.Range(0.3f, 1.5f) * direction[Random.Range(0, 2)]
        );

        Vector3 spawnPosition = player.position + randomPosition;

        // Check if the position is inside the mesh
        if (IsPositionInsideMesh(spawnPosition))
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
            enemiesList.Add(newEnemy);
        }
    }

    private bool IsPositionInsideMesh(Vector3 position)
    {
        if (meshManager == null) return false;

        foreach (var meshFilter in meshManager.meshes)
        {
            Mesh mesh = meshFilter.mesh;
            if (mesh == null) continue;

            // Use the bounds of the mesh to check if the position is inside
            if (mesh.bounds.Contains(position - meshFilter.transform.position))
            {
                //Debug.Log("N2R: position is inside mesh");
                return true;
            }
        }

        //Debug.Log("N2R: position is outside mesh");
        return false;
    }

    private void ValidateEnemies()
    {
        for (int i = enemiesList.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemiesList[i];

            // Check if the enemy is reachable
            if (!IsEnemyReachable(enemy))
            {
                enemiesList.Remove(enemy);
                enemyCount--;
                Debug.Log("N2R: removed enemy. Count: " + enemiesList.Count + "   Capacity: " + enemiesList.Capacity );
            }
        }
    }

    private bool IsEnemyReachable(GameObject enemy)
    {
        // Perform a line-of-sight check
        Vector3 direction = (enemy.transform.position - player.position).normalized;
        if (Physics.Raycast(player.position, direction, out RaycastHit hit))
        {
            if (hit.transform.gameObject == enemy)
            {
                return true; // Enemy is reachable
            }
        }

        return false; // Enemy is unreachable
    }

    public void removeEnemy(GameObject newEnemy){
        enemiesList.Remove(newEnemy);
        kills++;
        enemyCount--;
        Debug.Log("N2R: killed enemy. Count: " + enemiesList.Count + "   Capacity: " + enemiesList.Capacity );
    }

    public int getKills() {
        return kills;
    }
}