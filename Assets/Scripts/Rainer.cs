using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rainer : MonoBehaviour {
    public const float transitionTimeInSeconds = 0.05f;
    
    [Header("Spawnable objects")]
    [SerializeField] private GameObject[] treasurePrefabs;
    [SerializeField] private GameObject[] hazardPrefabs;

    [Header("Spawning parameters")]
    [SerializeField] private int minSpawnCooldownInSteps = 2;
    [SerializeField] private int maxSpawnCooldownInSteps = 4;
    [SerializeField] private int minSpawnHeight = 4;
    [SerializeField] private int maxSpawnHeight = 6;
    
    private int stepsUntilNextSpawn = 2;
    private HashSet<PlatformBlock> platformBlocks;
    private HashSet<PlatformBlock> possibleSpawningBlocks;
    private List<PlatformBlock> platformsWithFallingObject = new List<PlatformBlock>();
    
    void Start() {
        platformBlocks = new HashSet<PlatformBlock>(FindObjectsOfType<PlatformBlock>());
        possibleSpawningBlocks = new HashSet<PlatformBlock>(platformBlocks);
    }

    private void Spawn() {
        if (treasurePrefabs == null || treasurePrefabs.Length <= 0) {
            return;
        }
        if (possibleSpawningBlocks.Count <= 0) {
            return;
        }
        var platformBlock = possibleSpawningBlocks.ElementAt(Random.Range(0, possibleSpawningBlocks.Count));
        possibleSpawningBlocks.Remove(platformBlock);
        int spawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight + 1);
        float chance = Random.Range(0f, 1f);
        GameObject spawnedObject;
        if (chance > .5f) { // todo randomize & tweak hazard and treasure spawning
            spawnedObject = Instantiate(
                treasurePrefabs[Random.Range(0, treasurePrefabs.Length)],
                platformBlock.transform.position + new Vector3(0, spawnHeight, 0),
                Random.rotation);
        } else {
            spawnedObject = Instantiate(
                hazardPrefabs[Random.Range(0, hazardPrefabs.Length)],
                platformBlock.transform.position + new Vector3(0, spawnHeight, 0),
                Quaternion.identity);
        }
        
        var fallingObject = spawnedObject.GetComponent<FallingObject>();
        if (fallingObject) {
            platformBlock.SetFallingObject(fallingObject);
            platformsWithFallingObject.Add(platformBlock);
        }
    }

    public void Fall() {
        var finished = new List<PlatformBlock>();
        foreach (var platformBlock in platformsWithFallingObject) {
            if (platformBlock.FallAndStopIfNecessary()) {
                finished.Add(platformBlock);
            }
        }
        platformsWithFallingObject.RemoveAll(block => finished.Contains(block));
        stepsUntilNextSpawn--;
        if (stepsUntilNextSpawn <= 0) {
            Spawn();
            stepsUntilNextSpawn = Random.Range(minSpawnCooldownInSteps, maxSpawnCooldownInSteps + 1);
        }
    }

    public void ObjectPickedUp(FallingObject fallingObject) {
        possibleSpawningBlocks.Add(fallingObject.GetPlatform());
    }
}