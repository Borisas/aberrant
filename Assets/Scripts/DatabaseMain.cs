using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class DatabaseMain : ScriptableObject {

    public List<EnemyConfiguration> Enemies;
    
    public EnemyConfiguration GetEnemyById(EnemyId id) {
        for (int i = 0; i < Enemies.Count; i++) {
            if (Enemies[i].Id == id) {
                return Enemies[i];
            }
        }

        return null;
    }

#if UNITY_EDITOR
    [Button]
    void AutoCollectAssets() {
        Enemies = NTUtils.FindAssetsByType<EnemyConfiguration>().ToList();
    }
#endif
}
