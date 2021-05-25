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
    [SerializeField] private int minSpawnCooldownInSteps = 2;
    [SerializeField] private int maxSpawnCooldownInSteps = 4;
    [SerializeField] private float spawnHeight = 5f;
    
    private int stepsUntilNextSpawn = 2;
    private HashSet<Vector3> spawnPositions;

    private List<FallingObject> fallingObjects = new List<FallingObject>();
    
    void Start() {
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

    private void Spawn() {
        // todo randomize hazard and treasure spawning
        if (treasurePrefabs == null || treasurePrefabs.Length <= 0) {
            return;
        }
        var spawnedObject = Instantiate(
            treasurePrefabs[Random.Range(0, treasurePrefabs.Length)],
            spawnPositions.ElementAt(Random.Range(0, spawnPositions.Count)),
            Random.rotation);
        var fallingObject = spawnedObject.GetComponent<FallingObject>();
        if (fallingObject) {
            fallingObjects.Add(fallingObject);
        }
    }

    public void Fall() {
        foreach (var fallingObject in fallingObjects) {
            fallingObject.Fall();
        }
        stepsUntilNextSpawn--;
        if (stepsUntilNextSpawn <= 0) {
            Spawn();
            stepsUntilNextSpawn = Random.Range(minSpawnCooldownInSteps, maxSpawnCooldownInSteps + 1);
        }
    }

    public void Remove(FallingObject instance) {
        fallingObjects.Remove(instance);
    }
}