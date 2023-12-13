using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    //public Transform target;
    public PlayerController player;

    public bool followX = true;
    //desviaci�n de la posicion en el eje X, para dar margen de vision
    [Range(-2, 2)]
    public float offsetX = 1;

    public bool followY = false;
    //desviacion de la posicion en Y para dar margen de vision
    [Range(-2, 2)]
    public float offsetY = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        if (followX) newPos.x = player.transform.position.x + offsetX;
        if (followY) newPos.y = player.transform.position.y + offsetY;

        transform.position = newPos;
       
        if(player.transform.position.y <= -1)
        {
            followY = false;

        }
        else
        {
            followY = true;
        }

    }
}
