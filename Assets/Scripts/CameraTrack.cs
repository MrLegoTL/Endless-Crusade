using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform target;

    public bool followX = true;
    //desviaci�n de la posicion en el eje X, para dar margen de vision
    [Range(-2, 2)]
    public float offsetX = 1;

    public bool followY = false;
    //desviacion de la posicion en Y para dar margen de vision
    [Range(-2, 2)]
    public float offsetY = 0;

    

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        if (followX) newPos.x = target.position.x + offsetX;
        if (followY) newPos.y = target.position.y + offsetY;

        transform.position = newPos;

    }
}
