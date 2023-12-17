using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGem : PickUp
{
    //public float moreDamage;
    public float timeMoreDamage;
    public void Collect()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.ActivatePowerUp(timeMoreDamage);
    }
    private void OnDestroy()
    {
       Collect();
        
    }
    //private IEnumerator TimeMoreDamage()
    //{
    //    PlayerController player = FindObjectOfType<PlayerController>();
    //    player.damageAttack += moreDamage;
    //    yield return new WaitForSeconds(10);
    //}
}
