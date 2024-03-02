using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    
    [SerializeField] private Transform _enemyParent = null;
    [SerializeField] private Area2D _enemySpawnArea = null;
    
    public Enemy SpawnEnemy() {
        var p = _enemySpawnArea.GetRandomPoint();
        var cfg = Database.GetInstance().Main.GetEnemyById(EnemyId.Blob);
        var go = Instantiate(cfg.Prefab, _enemyParent);
        go.transform.position = p;
        go.Setup(cfg);
        // _enemies.Add(go);
        return go;
    }
    
}