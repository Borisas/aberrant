using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationConfiguration : ScriptableObject {
    public bool Obtainable = true;
    public MutationId Id;
    public string Title;
    public Player.Limb Limb;
    public int MaxLevel;
    public int Tier;
    public Sprite Icon;
    public Sprite Visual;
    public bool Attach;
    public float[] Values;
    public float[] ScaleValues;
}
