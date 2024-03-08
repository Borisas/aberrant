using UnityEngine;

public class GameConfig : ScriptableObject { 

    [Header("Prices")]
    public int PriceRecovery;
    public int PriceMutate;
    [Header("Blood Drops")]
    public float MinBloodDropBase = 3.0f;
    public float MaxBloodDropBase = 5.0f;
}