using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseMain : ScriptableObject {

    public GameObject HealthBar;
    public TMP_Text DamageNumberLabel;
    public List<EnemyConfiguration> Enemies;
    public List<PlayerMutationConfiguration> PlayerMutations;
    public List<Utils.LimbSprite> LimbIcons;
    public SpriteRenderer MutationInstance;
    public Material MaterialCharacterRegular;
    public Material MaterialCharacterElite;
    [Header("Configs")]
    public WaveControllerConfig WaveControllerConfig;
    public GameConfig GameConfig;
    [Header("Mutation Effects")] 
    public Projectile SpikeProjectile;
    public DamagingZone PoisonCloud;
    public GameObject BloodCross;
    
    public EnemyConfiguration GetEnemyById(EnemyId id) {
        for (int i = 0; i < Enemies.Count; i++) {
            if (Enemies[i].Id == id) {
                return Enemies[i];
            }
        }

        return null;
    }

    public PlayerMutationConfiguration GetMutationConfig(MutationId id) {
        for (int i = 0; i < PlayerMutations.Count; i++) {
            if (PlayerMutations[i].Id == id) {
                return PlayerMutations[i];
            }
        }

        return null;
    }

    public Sprite GetSpriteForLimb(Player.Limb limb) {
        for ( int i = 0; i < LimbIcons.Count; i++ ) {
            if ( LimbIcons[i].Limb == limb ) return LimbIcons[i].Sprite;
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
