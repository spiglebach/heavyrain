using System.Collections;
using System.Collections.Generic;
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
    
    private float timeUntilNextSpawn = 2.5f;
    
    void Start() {
        int xSize = maxX - minX + 1;
        int zSize = maxZ - minZ + 1;
        bool[,] blocks = new bool[xSize, zSize]; // todo use list to ease random position assignment
        Debug.Log("[" +blocks.GetLength(0) + "," + blocks.GetLength(1) + "]");
        for (int x = minX; x <= maxX; x++) {
            for (int z = minZ; z <= maxZ; z++) {
                Vector3 rayStart = new Vector3(x, 1, z);
                Vector3 down = Vector3.down;
                RaycastHit hit;
                if (Physics.Raycast(rayStart, down, out hit, 2, LayerMask.GetMask("Platforms"))) {
                    blocks[xSize - maxX + x - 1, zSize - maxZ + z - 1] = true;
                    Debug.Log("something found at: " + (rayStart + down).ToString());
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
        if (treasurePrefabs == null || treasurePrefabs.Length <= 0) {
            return;
        }
        Instantiate(treasurePrefabs[0], new Vector3(0, 5, 0), Random.rotation); // todo assign random position on grid
    }
}