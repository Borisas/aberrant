using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour {

    [SerializeField] private Transform _spawnTarget = null;
    private Transform _previousParent = null;
    private Vector3 _posPrevious = default;
    private Actor _actor = null;
    
    public void Spawn(Actor a) {
        _actor = a;
        
        _actor.SetActing(false);
        _previousParent = _actor.transform.parent;

        _posPrevious = _actor.transform.position;
        _actor.transform.SetParent(_spawnTarget);
        _actor.transform.localPosition = Vector3.zero;
    }

    public void OnCompleted() {
        _actor.SetActing(true);
        _actor.transform.SetParent(_previousParent);
        // _actor.transform.position = _posPrevious;
        gameObject.SetActive(false);
    }

}
