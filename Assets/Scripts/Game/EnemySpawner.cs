using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    
    [SerializeField] private Transform _enemyParent = null;
    [SerializeField] private Area2D[] _enemySpawnArea = null;

    Area2D[] _selectableAreas = null;
    Area2D _lastArea = null;

    private List<Enemy> _livingEnemies = new List<Enemy>();

    void Awake() {
        _selectableAreas = new Area2D[_enemySpawnArea.Length-1];
    }
    
    //only this should spawn an enemy.
    public Enemy SpawnEnemy(bool elite, EnemyId id) {
        var p = GetArea().GetRandomPoint();
        var cfg = Database.GetInstance().Main.GetEnemyById(id);
        var go = ObjectPool.Get(cfg.Prefab, _enemyParent);
        go.Setup(cfg, elite);
        go.transform.position = p;
        _livingEnemies.Add(go);
        go.OnDie += Enemy_OnDie;
        return go;
    }

    public Enemy SpawnEnemyNotNewArea(bool elite, EnemyId id) {

        if ( _lastArea == null ) {
            GenerateNewArea();
        }

        var p = _lastArea.GetRandomPoint();
        var cfg = Database.GetInstance().Main.GetEnemyById(id);
        var go = ObjectPool.Get(cfg.Prefab, _enemyParent);
        go.Setup(cfg, elite);
        go.transform.position = p;
        _livingEnemies.Add(go);
        go.OnDie += Enemy_OnDie;
        return go;
    }

    public void GenerateNewArea() {
        GetArea();
    }


    Area2D GetArea() {
        if ( _lastArea == null ) {
            _lastArea = _enemySpawnArea.Random();
        }
        else {
            ConstructSelectableAreas();
            _lastArea = _selectableAreas.Random();
        }
            return _lastArea;
    }

    void ConstructSelectableAreas() {
        
        int k = 0;
        for ( int i = 0; i < _enemySpawnArea.Length; i++ ) {
            if ( _enemySpawnArea[i] == _lastArea ) continue;
            _selectableAreas[k] = _enemySpawnArea[i];
            k++;
        }
    }

    private void Enemy_OnDie(Actor enemy) {
        _livingEnemies.Remove(enemy as Enemy);
    }

    public int GetLivingEnemyCount() {
        int count = 0;
        for (int i = 0; i < _livingEnemies.Count; i++) {
            if (_livingEnemies[i].IsAlive()) count++;
        }
        return count;
    }

    public IEnumerable<Enemy> GetEnemies() => _livingEnemies;
}