using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [Header("PlayerCheck")]
    public Transform playerCheck;
    public Vector2 sizePlayerCheck = new Vector2(0.16f, 0.03f);


    [Header("Movement")]
    public bool autoMove = true;
    public float maxSpeed = 1f;
    public float acceleration = 3.5f;
    public Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerCheck.position, sizePlayerCheck);
    }
    /// <summary>
    /// Metodo para que la niebla se mueva automaticamente hacia el jugador
    /// </summary>
    public void Movement()
    {
        if (!autoMove) return;

        Vector2 tempVelocity = rigidBody.velocity;

        tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, maxSpeed, acceleration * Time.deltaTime);

        
        rigidBody.velocity = tempVelocity;
    }
}
