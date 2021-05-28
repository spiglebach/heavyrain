using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {
    private Rainer rainer;
    
    private void Start() {
        rainer = FindObjectOfType<Rainer>();
    }

    private void OnTriggerEnter(Collider other) {
        var otherGameObject = other.gameObject;
        var fallingObject = otherGameObject.GetComponent<FallingObject>();
        if (!fallingObject) return;
        //rainer.Remove(fallingObject);
        //Destroy(otherGameObject);
    }
}
