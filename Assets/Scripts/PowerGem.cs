using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGem : PickUp
{
    public float moreDamage;
    public void Collect()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.damageAttack += moreDamage;
    }
    private void OnDestroy()
    {
        Collect();
    }
}
