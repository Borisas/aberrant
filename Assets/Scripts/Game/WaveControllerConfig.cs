using UnityEngine;

public class WaveControllerConfig : ScriptableObject { 
    [Header("Weight")]
    public float MinWeight = 1;
    public float MaxWeight = 2;
    public float MinWeightPerWave = 1;
    public float MaxWeightPerWave = 2;
    public float HighSpawnLimit = 0.85f;
    public float HighSpawnReductor = 0.5f;
    [Header("Delay")]
    public float MinDelay = 1.25f;
    public float MaxDelay = 2.0f;
    public float MaxDelayLossPerWave = 0.05f;
    public float FirstSpawnDelay = 0.0f;
    public float HighSpawnDelayIncrease = 1.5f;
    [Header("Other")]
    public float EliteChance = 0.15f;
    public int ElitesFromWave = 3;
}