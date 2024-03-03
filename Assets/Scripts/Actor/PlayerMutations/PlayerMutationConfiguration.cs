using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationConfiguration : ScriptableObject {
    public PlayerMutationId Id;
    public Player.Limb Limb;
    public Sprite Icon;
    public Sprite Visual;
    public bool Attach;
}
