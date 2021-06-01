using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedPlatformBlock : PlatformBlock {
    [SerializeField] private GameObject spawnablePrefab;
    [SerializeField] private int spawnHeight;
    protected override void Start() {
        base.Start();
        FindObjectOfType<Rainer>().SpawnScripted(this);
    }

    public GameObject GetSpawnablePrefab() {
        return spawnablePrefab;
    }

    public int GetSpawnHeight() {
        return spawnHeight;
    }

}
