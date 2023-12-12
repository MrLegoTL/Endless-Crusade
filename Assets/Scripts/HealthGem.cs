using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGem : PickUp
{
    public float healthToRestore;
   public void Collect()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.RestoredHealth(healthToRestore);
    }
    private void OnDestroy()
    {
        Collect();
    }
}
