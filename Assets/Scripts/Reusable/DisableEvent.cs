using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEvent : MonoBehaviour {

    public void Disable() {
        gameObject.SetActive(false);
    }
}
