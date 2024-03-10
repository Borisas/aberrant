using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {

        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var actor = rb.GetComponent<Actor>();
        if (actor == null) return;

        actor.ForceDie();
    }
}
