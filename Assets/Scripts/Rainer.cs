using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainer : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private int minX = 0;
    [SerializeField] private int minZ = 0;
    [SerializeField] private int maxX = 5;
    [SerializeField] private int maxZ = 5;
    void Start() {
        int xSize = maxX - minX + 1;
        int zSize = maxZ - minZ + 1;
        bool[,] blocks = new bool[xSize, zSize];
        Debug.Log("[" +blocks.GetLength(0) + "," + blocks.GetLength(1) + "]");
        for (int x = minX; x <= maxX; x++) {
            for (int z = minZ; z <= maxZ; z++) {
                Vector3 rayStart = new Vector3(x, 1, z);
                Vector3 down = Vector3.down;
                RaycastHit hit;
                if (Physics.Raycast(rayStart, down, out hit, 2)) {
                    blocks[xSize - maxX + x - 1, zSize - maxZ + z - 1] = true;
                    Debug.Log("something found at: " + (rayStart + down).ToString());
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() { }
}