using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortingOrderOverY : MonoBehaviour {
    
    private SortingGroup _sorting = null;
    
    private void Awake() {
        _sorting = GetComponent<SortingGroup>();
    }

    private void LateUpdate() {
        _sorting.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100.0f);
    }
}
