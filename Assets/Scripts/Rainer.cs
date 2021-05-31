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

    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private GameObject[] hazardPrefabs;

    [Header("Spawning parameters")]
    [SerializeField] private int minSpawnCooldownInSteps = 2;
    [SerializeField] private int maxSpawnCooldownInSteps = 4;
    [SerializeField] private int minSpawnHeight = 3;
    [SerializeField] private int maxSpawnHeight = 6;
    [SerializeField] private Transform fallingObjectsParent;
    [SerializeField] private float hazardSpawnUpperThreshold = 0.3f;
    [SerializeField] private float powerUpSpawnUpperThreshold = 0.5f;
    [SerializeField] private bool canSpawnHazard;
    [SerializeField] private bool canSpawnPowerUp;
    
    private int stepsUntilNextSpawn = 2;
    private HashSet<PlatformBlock> platformBlocks;
    private HashSet<PlatformBlock> possibleSpawningBlocks;
    private List<PlatformBlock> platformsWithFallingObject = new List<PlatformBlock>();
    
    void Start() {
        platformBlocks = new HashSet<PlatformBlock>(FindObjectsOfType<PlatformBlock>());
        possibleSpawningBlocks = new HashSet<PlatformBlock>(platformBlocks);
        canSpawnHazard = canSpawnHazard && hazardPrefabs.Length > 0;
        canSpawnPowerUp = canSpawnPowerUp && powerUpPrefabs.Length > 0;
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
        Vector3 spawnPosition = platformBlock.transform.position + new Vector3(0, spawnHeight, 0);
        if (canSpawnHazard && chance <= hazardSpawnUpperThreshold) {
            var hazard = hazardPrefabs[Random.Range(0, hazardPrefabs.Length)];
            spawnedObject = Instantiate(
                hazard,
                spawnPosition,
                hazard.transform.rotation);
        } else if (canSpawnPowerUp && chance <= powerUpSpawnUpperThreshold) {
            var powerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            spawnedObject = Instantiate(
                powerUp,
                spawnPosition,
                powerUp.transform.rotation);
        } else {
            var treasure = treasurePrefabs[Random.Range(0, treasurePrefabs.Length)];
            spawnedObject = Instantiate(
                treasure,
                spawnPosition,
                treasure.transform.rotation);
        }
        spawnedObject.transform.SetParent(fallingObjectsParent);
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