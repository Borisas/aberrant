using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DatabaseMain : ScriptableObject {

    public GameObject HealthBar;
    public TMP_Text DamageNumberLabel;
    public List<EnemyConfiguration> Enemies;
    public List<PlayerMutationConfiguration> PlayerMutations;
    
    public EnemyConfiguration GetEnemyById(EnemyId id) {
        for (int i = 0; i < Enemies.Count; i++) {
            if (Enemies[i].Id == id) {
                return Enemies[i];
            }
        }

        return null;
    }

    public PlayerMutationConfiguration GetMutationConfig(PlayerMutationId id) {
        for (int i = 0; i < PlayerMutations.Count; i++) {
            if (PlayerMutations[i].Id == id) {
                return PlayerMutations[i];
            }
        }

        return null;
    }

#if UNITY_EDITOR
    [Button]
    void AutoCollectAssets() {
        Enemies = NTUtils.FindAssetsByType<EnemyConfiguration>().ToList();
        PlayerMutations = NTUtils.FindAssetsByType<PlayerMutationConfiguration>().ToList();
    }
#endif
}
