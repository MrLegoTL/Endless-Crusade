using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    private float horizontal;
    public float speed;
    private bool isFancingRight = true;
    public bool canMove = true;

    [Header("Roll")]
    public float rollSpeed;
    public float rollTime;
    public bool canRoll = true;
    //public float dashCooldown;





    [Header("Jump Settings")]
    public bool canJump = true;
    public bool jump = false;
    public float jumpForce = 0.8f;
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    [Header("Jump Hang Time Settings")]
    public float hangTimeDuration = 0.5f;
    private bool isJumping = false;


    public Rigidbody2D rigidBody;


    [Header("GroundCheck")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 sizeGroundCheck = new Vector2(0.16f, 0.03f);
    public bool grounded = false;
    //limite de muerte
    public float deadlimit = -1.5f;


    //[Header("Movement")]
    ////public bool autoMove = true;
    //public float maxSpeed = 1f;
    //public float acceleration = 3.5f;


    //referencia al animator
    public Animator animator;
    public bool isDead = false;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EvaluatedGrounded();
        Movement();
        CheckDeadLimit();
        FeedAnimation();
        

        ////Disminuye el contador de Coyote Time
        //coyoteTimeCounter-= Time.deltaTime;

        //// Si se está saltando y no se está en el suelo, aplica la técnica de Jump Hang Time
        //if (isJumping && !grounded)
        //{
        //    // Inicia la corutina de Jump Hang Time
        //    StartCoroutine(JumpHangTimeCoroutine());
        //}


    }

    public void Movement()
    {
        // si esta desactivado el canMove, no hacemos nada en este m�todo
        if (!canMove) return;
        if (canMove)
        {
            rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
            if (!isFancingRight && horizontal > 0f)
            {
                Flip();
            }
            else if (isFancingRight && horizontal < 0f)
            {
                Flip();
            }
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        
        if (canRoll && grounded)
        {
            StartCoroutine(Roll());
        }
    }


    private IEnumerator Roll()
    {
        canMove = false;
        canRoll = false;
        canJump = false;
        rigidBody.velocity = new Vector2(rollSpeed * transform.localScale.x, 0f);

        yield return new WaitForSeconds(rollTime);
        canMove = true;
        canRoll = true;
        canJump=true;
       // yield return new WaitForSeconds(dashCooldown);
       

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, sizeGroundCheck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DamageZone")
        {
            Dead();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //si el objetos colisionado es un enemigo 

        if (collision.tag == "Enemy")
        {
            //indicamos al jugador que muera
            Dead();
        }
    }

   

    private void Flip()
    {
        isFancingRight = !isFancingRight;
        Vector3 localScale  =transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        horizontal=context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && canJump)
        {
            Jump();
            
        }
        else if (context.canceled)
        {
            isJumping = false;
            
        }




    }

    public void Jump()
    {
        ////Si no estamos tocando el suelo abandonamos el metedo sin hacer nada
        //if (!grounded) return;

        ////reseteamos la velocidad vertical actual
        //rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

        ////aplicamos un impulso vertical hacia arriba
        //rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);


        //Verifica que el player esta tocando el suelo  y si esta dentro del Coyote Time
        if (grounded || coyoteTimeCounter > 0 )
        {
            
            //reseteamos la velocidad vertical actual
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            //aplicamos un impulso vertical hacia arriba
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            //reinicia el contador de Coyote Time
            coyoteTimeCounter = 0;

            // Establece que el jugador está en proceso de salto
            isJumping = true;




        }
        else //no esta tocando suelo
        {
            // No está tocando el suelo, pero se intenta saltar
            isJumping = true;
            
        }
        //Disminuye el contador de Coyote Time
        coyoteTimeCounter -= Time.deltaTime;

        // Si se está saltando y no se está en el suelo, aplica la técnica de Jump Hang Time
        if (isJumping && !grounded)
        {
            // Inicia la corutina de Jump Hang Time
            StartCoroutine(JumpHangTimeCoroutine());
        }




    }

    IEnumerator JumpHangTimeCoroutine()
    {
        // Reduce la gravedad para simular el Jump Hang Time
        rigidBody.gravityScale = 0.5f;

        // Espera el tiempo de hang time
        yield return new WaitForSeconds(hangTimeDuration);

        // Restaura la gravedad
        rigidBody.gravityScale = 1f;

        // Establece que el jugador ya no está en proceso de salto
        isJumping = false;
    }




    private void EvaluatedGrounded()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, sizeGroundCheck, 0f, groundLayer);
        rigidBody.gravityScale = grounded ? 0 : 1;
        
        if (grounded)
        {
            //Actualiza el contador cuando el player toca el suelo
            coyoteTimeCounter = coyoteTime;
            
        }

    }

    /*public void Movement()
    {
        // si esta desactivado el autoMove, no hacemos nada en este m�todo
        if (!autoMove) return;

        //almaceno la velocidad actual del rigidbody en una variable temporal
        Vector2 tempVelocity = rigidBody.velocity;

        //modificamos el valor x (horizontal) par aproximarlo a la velociadad objetivo, mediante una aceleracion progresiva
        //deberemos multiplicar la aceleracion por el delta time, para que se aplique la aceleraci�n correspondiente
        //a la fracci�n de tiempo transcurrida desde el �ltimo frame
        //de esta forma la aceleraci�n ser� igual independientemente de los FPS a los que se renderice el juego
        tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, maxSpeed, acceleration * Time.deltaTime);

        //asignamos la velocidad calculada
        rigidBody.velocity = tempVelocity;
    }*/

    private void FeedAnimation()
    {
        //indicamos al animator cuando el player se encuentra en contacto con el suelo
        animator.SetBool("Grounded", grounded);
        //le transmito el valor absoluto de la velocidad en el eje x
        animator.SetFloat("Velocity", Mathf.Abs(rigidBody.velocity.x));

        animator.SetBool("Roll", !canMove);

       

    }

    public void Dead()
    {
        //si el jugador ya estaba muerto no hacemos nada
        if(isDead) return;
        //Indicaos que el jugador ha muerto
        isDead = true;
        //dejamos de aplicar movimiento
        canMove= false;
        //detenemos toda velocidad residual
        rigidBody.velocity = Vector2.zero;
        //Indicamos al animator que ejecute la animcaion de muerte
        animator.SetBool("Dead", true);

    }

    /// <summary>
    /// verifica si el jugador cae por debajo de limite mortal
    /// </summary>
    private void CheckDeadLimit()
    {
        if(transform.position.y <=deadlimit) Dead();
    }

}

