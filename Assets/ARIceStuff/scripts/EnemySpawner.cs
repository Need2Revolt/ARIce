// 23/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EnemySpawner : MonoBehaviour
{
    public float startingDelay = 10;
    public float delayDecrease = 0.5f;

    public int maxEnemies = 10;

    public GameObject[] enemyPrefabs;

    public List<GameObject> enemiesList;

    private float timer;

    public int[] direction = { 1, -1 };

    private int kills;

    public ARMeshManager meshManager; // Reference to the ARMeshManager for accessing the mesh data
    public Transform player; // Reference to the player's transform

    // Start is called before the first frame update
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
        if (timer > startingDelay)
        {
            if (enemiesList.Count < maxEnemies)
            {
                SpawnEnemy();
                startingDelay -= delayDecrease;
                timer = 0;
            }
        }
    }

    private void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        // Generate a random position around the player
        Vector3 randomPosition = new Vector3(
            Random.Range(0.4f, 2f) * direction[Random.Range(0, 2)],
            Random.Range(0, 1),
            Random.Range(0.4f, 2f) * direction[Random.Range(0, 2)]
        );

        Vector3 spawnPosition = player.position + randomPosition;

        // Check if the position is inside the mesh
        if (IsPositionInsideMesh(spawnPosition))
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
            enemiesList.Add(newEnemy);
        }
        else
        {
            // Position is outside the mesh, do not spawn the enemy
            kills++;
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
                Debug.Log("N2R: position is inside mesh");
                return true;
            }
        }

        Debug.Log("N2R: position is outside mesh");
        return false;
    }

    public void removeEnemy(GameObject newEnemy){
        enemiesList.Remove(newEnemy);
        Debug.Log("N2R: removed enemy. Count: " + enemiesList.Count + "   Capacity: " + enemiesList.Capacity );
        kills++;
    }

    public int getKills() {
        return kills;
    }
}