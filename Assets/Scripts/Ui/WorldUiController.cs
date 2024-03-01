using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUiController : MonoBehaviour {

    [SerializeField] private RectTransform _healthBarParent = null;
    [SerializeField] private DamageNumbers _damageNumbers;

    public RectTransform GetHealthBarParent() => _healthBarParent;
    public DamageNumbers GetDamageNumbers() => _damageNumbers;

    private void Awake() {
        
        for (int i = 0; i < _healthBarParent.childCount; i++) {
            Destroy(_healthBarParent.GetChild(i));
        }
    }
}
