using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    //private void OnTriggerEnter2D(Collider col)
    //{
    //    //Compruba si el item colisiona con algo con el tag Player
    //    if (col.CompareTag("Player"))
    //    {
    //        // el item es destruido
    //        Destroy(gameObject);
    //    }
    //}

}
