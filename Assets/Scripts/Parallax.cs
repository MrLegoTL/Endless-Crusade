using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //velocidad del fondo, con 0.5 igualariamos a la de la carrera
    [Range(0, 0.5f)]
    public float speedFactor = 0.066f;
    //posicion para control del offset de la textura
    private Vector2 pos = Vector2.zero;
    //referencia al main camera
    private Camera cam;
    //posicion anterior de la cámara
    private Vector2 camOldPos;
    //referencia al renderer del background para acceder a su material
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //inicializamos la posicion anterior de la camara con la posicion actual
        camOldPos = cam.transform.position;
        //recuperamos la referencia al componente renderer
        rend = GetComponent<Renderer>();

        // el ortographic Size es la mitad del alto de la camara
        //screen.widht es el ancho de la pantalla en pixeles
        //screen.height es el alto de la pantalla en pixeles

        //alturaOrtho -- anchuraOrtho
        //    height -- widht
        //anchuraOrtho = (alturaOrtho * widht)/height
        Vector2 backgroundHalfSize = new Vector2((cam.orthographicSize * Screen.width) / Screen.height, cam.orthographicSize);

        //ajustamos la escala del fondo para que se ajuste al tamaño de pantalla
        transform.localScale = new Vector3(backgroundHalfSize.x * 2,
                                           backgroundHalfSize.y * 2,
                                           transform.localScale.z);

        //ajustamos el tilling para que sea proporcionado de forma correcta a la escala del quad
        //lo dejamos a la mitad para reducir el numero de repeticiones (esto es una decision estetica)
        rend.material.SetTextureScale("_MainTex", backgroundHalfSize);
    }

    // Update is called once per frame
    void Update()
    {
        //calculamos el desplazamiento de la camara respecto al ciclo anterior
        Vector2 camVar = new Vector2(cam.transform.position.x - camOldPos.x, cam.transform.position.y - camOldPos.y);

        //modificamos el offset que se aplicara a la textura
        //lo multiplicamos por el speedFactor, para modificar su desplazamiento respecto a la camara
        pos.Set(pos.x + (camVar.x * speedFactor),
                pos.y + (camVar.y * speedFactor));

        //aplicamos el offset a la textura principal
        rend.material.SetTextureOffset("_MainTex", pos);

        //actualizamos la posicion de la camara para el siguiente ciclo
        camOldPos = cam.transform.position;


    }
}
