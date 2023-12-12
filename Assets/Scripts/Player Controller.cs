using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Enemy enemy;
    [Header("Move")]
    private float horizontal;
    public float speed;
    private bool isFancingRight = true;
    public bool canMove = true;

    [Header("Roll")]
    public float rollSpeed;
    public float rollTime;
    public bool canRoll = true;
    public bool isInvincible = false;
    //public float dashCooldown;

    [Header("Attack")]
    [SerializeField]
    private Transform attackManager;    
    [SerializeField]
    private float areaAttack;
    [SerializeField]
    public float damageAttack;

    [Header("AirAttack")]
    [SerializeField]
    private Transform airAttackManager;
    [SerializeField]
    private float airAreaAttack;
    [SerializeField]
    private float airDamageAttack;


    public bool canAttack = true;
    public bool canCombo = false;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
  

    [Header("Jump Settings")]
    public bool canJump = true;
    public float jumpForce = 0.8f;
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;
    private bool isJumping = false;
    public float newGravity;
    


    public Rigidbody2D rigidBody;


    [Header("GroundCheck")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 sizeGroundCheck = new Vector2(0.16f, 0.03f);
    public bool grounded = false;
    //limite de muerte
    public float deadlimit = -1.5f;
    public GameObject fogDeadLimit;

    [Header("Health")]
    [SerializeField]
    private float playerHealth;
    [SerializeField]
    private float playerMaxHealth;
    [SerializeField]
    private HealthBar healthBar;

    [Header("Climb")]
    [SerializeField]
    private float climbSpeed;
    private BoxCollider2D boxCollider2D;
    private float initialGravity;
    private bool isClimbing;
    private float vertical;
    private Vector2 input;


    //referencia al animator
    public Animator animator;
    public bool isDead = false;
    // Start is called before the first frame update

    void Start()
    {
        enemy = GetComponent<Enemy>();
        playerHealth = playerMaxHealth;
        healthBar.InitializeHealthBar(playerHealth);
        boxCollider2D = GetComponent<BoxCollider2D>();
        initialGravity = rigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        EvaluatedGrounded();
        Movement();
        CheckDeadLimit();
        FeedAnimation();
        

        ////Disminuye el contador de Coyote Time
        coyoteTimeCounter-= Time.deltaTime;

        input.y = Input.GetAxisRaw("Vertical");
       

    }
    private void FixedUpdate()
    {
        Climb();
    }

    public void Movement()
    {
        // si esta desactivado el canMove, no hacemos nada en este m�todo
        //if (!canMove) return;
       
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(Time.time >= nextAttackTime)
        {
            if (context.performed)
            {
                Golpe();
                nextAttackTime = Time.time+1f/attackRate;

            }
        }
        
        //if (context.performed )
        //{

        //    if (canAttack)
        //    {
        //        StartCoroutine(Attack());
        //    }
            
             
            
        //}
      
        
    
    }

    /// <summary>
    /// Metodo para el ataque de player
    /// </summary>
    /// 
    void Golpe()
    {
        animator.SetTrigger("Golpe");

        Collider2D[] objects = Physics2D.OverlapCircleAll(attackManager.position, areaAttack);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<Enemy>().TakeDamage(damageAttack);
            }
        }


    }
    //private IEnumerator Attack()
    //{
    //    canMove = false;
    //    canAttack = false;

    //    Collider2D[] objects = Physics2D.OverlapCircleAll(attackManager.position, areaAttack);

    //    foreach (Collider2D collider in objects)
    //    {
    //        if (collider.CompareTag("Enemy"))
    //        {                
    //            collider.transform.GetComponent<Enemy>().TakeDamage(damageAttack);

    //        }
    //    }

    //    yield return new WaitForSeconds(1f);

    //    canAttack = true;
    //    canMove = true;
    //}




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
        isInvincible = true;
        rigidBody.velocity = new Vector2(rollSpeed * transform.localScale.x, 0f);
        

        yield return new WaitForSeconds(rollTime);
        isInvincible = false;
        canMove = true;
        canRoll = true;
        canJump=true;
       // yield return new WaitForSeconds(dashCooldown);
       

    }

    private void OnDrawGizmos()
    {
        //Gizmos para el groundCheck
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, sizeGroundCheck);
        //Gizmos para areaAttack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackManager.position, areaAttack);
        //Gizmos para airAreaAttack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(airAttackManager.position, airAreaAttack);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DamageZone" && !isInvincible)
        {
            Dead();
        }
        if (collision.CompareTag("PickUp"))
        {
            animator.SetTrigger("Health");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible || (canRoll && collision.collider.CompareTag("Enemy")))
        {
            return;
        }
       else if (collision.collider.CompareTag("Enemy") && !isInvincible)
        {
            //indicamos al jugador que muera
            Dead();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //si el objetos colisionado es un enemigo 

        if (collision.tag == "Enemy" && !isInvincible)
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
        if (canMove)
        {

        horizontal=context.ReadValue<Vector2>().x;
        }else 
        {
            horizontal = 0;
        }
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
        if (grounded || coyoteTimeCounter > 0)
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


    }

    public void OnAirAttack(InputAction.CallbackContext context)
    {
        if (context.performed )
        {
            
            AirAttack();
        }
    }
    void AirAttack()
    {
        if (!grounded)
        {
            canMove = false;
            
            animator.SetTrigger("AirAttack");
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);

            Collider2D[] objects = Physics2D.OverlapCircleAll(airAttackManager.position, airAreaAttack);

            foreach (Collider2D collider in objects)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.transform.GetComponent<Enemy>().TakeDamage(airDamageAttack);
                }
            }
        }
        else
        {
            canMove = true;
        }
       
    }



    private void EvaluatedGrounded()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, sizeGroundCheck, 0f, groundLayer);
        rigidBody.gravityScale = grounded ? 0 : newGravity;
        
        if (grounded)
        {
            //Actualiza el contador cuando el player toca el suelo
            coyoteTimeCounter = coyoteTime;
            
        }

    }

   

    private void FeedAnimation()
    {
        //indicamos al animator cuando el player se encuentra en contacto con el suelo
        animator.SetBool("Grounded", grounded);
        //le transmito el valor absoluto de la velocidad en el eje x
        animator.SetFloat("Velocity", Mathf.Abs(rigidBody.velocity.x));
        
        animator.SetBool("Roll", !canRoll);
        animator.SetBool("Attack", !canAttack);

        if(Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon)
        {
            animator.SetFloat("VelocityY", Mathf.Sign(rigidBody.velocity.y));
        }
        else
        {
            animator.SetFloat("VelocityY", 0);
        }




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
        if(transform.position.x <= fogDeadLimit.transform.position.x )
        {
            Dead();
        }
    }

    public void TakeDamage(float dmg)
    {
        playerHealth -= dmg;
        healthBar.ChangedCurrentHealth(playerHealth);
        animator.SetTrigger("Hurt");

        if (playerHealth <= 0) 
        {
            Dead();
        }
    }

    public void RestoredHealth(float restoredHealth)
    {
        if((playerHealth + restoredHealth) > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        else
        {
            playerHealth += restoredHealth;
            healthBar.ChangedCurrentHealth(playerHealth);
        }
    }

    //public void OnClimb(InputAction.CallbackContext context)
    //{
    //    //vertical = context.ReadValue<Vector2>().y;
    //    Climb();
    //}
    void Climb()
    {
        if((input.y !=0 || isClimbing) && (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Stairs"))))
        {
            Vector2 upSpeed = new Vector2(rigidBody.velocity.x, input.y * climbSpeed);
            rigidBody.velocity = upSpeed;
            rigidBody.gravityScale = 0;
            isClimbing = true;
        }
        else
        {
            rigidBody.gravityScale = initialGravity;
            isClimbing=false;
        }
        if (grounded)
        {
            isClimbing = false;
        }
        animator.SetBool("isClimbing", isClimbing);
    }

}

