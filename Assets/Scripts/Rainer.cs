using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rainer : MonoBehaviour {
    [Header("Map Constraints")]
    [SerializeField] private int minX = 0;
    [SerializeField] private int minZ = 0;
    [SerializeField] private int maxX = 5;
    [SerializeField] private int maxZ = 5;
    
    [Header("Spawnable objects")]
    [SerializeField] private GameObject[] treasurePrefabs;
    [SerializeField] private GameObject[] hazardPrefabs;

    [Header("Spawning parameters")]
    [SerializeField] private float minSpawnCooldownInSeconds = 1f;
    [SerializeField] private float maxSpawnCooldownInSeconds = 3f;
    [SerializeField] private float spawnHeight = 5f;
    
    private float timeUntilNextSpawn = 2.5f;
    private HashSet<Vector3> spawnPositions;
    
    void Start() {
        int xSize = maxX - minX + 1;
        int zSize = maxZ - minZ + 1;
        spawnPositions = new HashSet<Vector3>();
        for (int x = minX; x <= maxX; x++) {
            for (int z = minZ; z <= maxZ; z++) {
                Vector3 rayStart = new Vector3(x, 1, z);
                Vector3 down = Vector3.down;
                RaycastHit hit;
                if (Physics.Raycast(rayStart, down, out hit, 2, LayerMask.GetMask("Platforms"))) {
                    spawnPositions.Add(new Vector3(x, spawnHeight, z));
                    Debug.Log("something found at: " + rayStart);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
        }
    }

    void Update() {
        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn <= 0) {
            Spawn();
            timeUntilNextSpawn = 1f; // todo provide new random spawn time
        }
    }

    private void Spawn() {
        // todo randomize hazard and treasure spawning
        if (treasurePrefabs == null || treasurePrefabs.Length <= 0) {
            return;
        }
        Instantiate(
            treasurePrefabs[Random.Range(0, treasurePrefabs.Length)],
            spawnPositions.ElementAt(Random.Range(0, spawnPositions.Count)),
            Random.rotation);
    }
}