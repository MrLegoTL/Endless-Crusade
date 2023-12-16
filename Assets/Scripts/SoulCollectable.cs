using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollectable : PickUp
{
    [SerializeField]
    private int soulValue;
    public void Collect()
    {
        GameManager.instance.PickupCollectable(soulValue);
    }
    private void OnDestroy()
    {
        Collect();
    }
}
