using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCompleteEvent : MonoBehaviour {

    public event System.Action Complete;

    public void OnCompleteAnim() {
        Complete?.Invoke();
        Complete = null;
    }
}
