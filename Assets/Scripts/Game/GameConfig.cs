using UnityEngine;

public class GameConfig : ScriptableObject { 

    [Header("Prices")]
    public int PriceRecovery;
    public int PriceRecoveryIncrease = 5;
    public int PriceMutate;
    public int PriceMutateIncrease = 10;
    public int PriceMoreLife = 20;
    public int PriceMoreLifeIncrease = 20;
    [Header("Blood Drops")]
    public float MinBloodDropBase = 3.0f;
    public float MaxBloodDropBase = 5.0f;
    [Header("Other")]
    public int TurretBaseDamage = 2;
    public int MoreLifeIncrease = 20;
}