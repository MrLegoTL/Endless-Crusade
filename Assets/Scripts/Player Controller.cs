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
    [SerializeField]
    private float timeBetweenAttacks;
    [SerializeField]
    private float timeNextAttack;
    [SerializeField]
    private float timeLastAttack;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private bool isSecondAttack = false;
    [SerializeField]
    private float timeFinishCombo = 0.5f;


    [Header("AirAttack")]
    [SerializeField]
    private Transform airAttackManager;
    [SerializeField]
    private float airAreaAttack;
    [SerializeField]
    private float airDamageAttack;
    [SerializeField]
    private bool isAirAttack;


    public bool canAttack = true;
    public bool canCombo = false;

    
  

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

    [Header("Cheats")]
    [SerializeField]
    private bool isImmune = false;
    private bool moreDamage = false;
    private bool moreJumpForce;

    [Header("PowerUp")]
    //para realizar el contador inteno de la duración del powerup
    private float powerUpCounter = 0f;
    //corrutina para gestionar la duración del powerUp
    private Coroutine powerUpCoroutine;

    //referencia al animator
    public Animator animator;
    public bool isDead = false;
    public bool isPickUp = false;
    // Start is called before the first frame update

    void Start()
    {
        enemy = GetComponent<Enemy>();
        playerHealth = playerMaxHealth;
        healthBar.InitializeHealthBar(playerHealth);
        boxCollider2D = GetComponent<BoxCollider2D>();
        initialGravity = rigidBody.gravityScale;
        timeNextAttack = timeBetweenAttacks;
        timeLastAttack= timeBetweenAttacks;
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

        if (isAttacking)
        {
            timeNextAttack -= Time.deltaTime;
        }
        if (isSecondAttack)
        {
            timeLastAttack -= Time.deltaTime;
        }
        
    }
    private void FixedUpdate()
    {
        Climb();
        if (timeNextAttack <= 0 )
        {
            isAttacking = false;
            timeNextAttack = timeBetweenAttacks;
            StartCoroutine(FinishCombo(0));
           
        }else if(timeLastAttack<=0)
        {
            isAttacking = false;
            isSecondAttack = false;
            timeLastAttack = timeBetweenAttacks;
            StartCoroutine(FinishCombo(0));
        }
    }

    public void Move(InputAction.CallbackContext context)
    {


        horizontal = context.ReadValue<Vector2>().x;

    }

    /// <summary>
    /// Metodo par ale movimiento del player
    /// </summary>
    public void Movement()
    {
        // si esta desactivado el canMove, no hacemos nada en este m�todo
        if (!canMove ) return;

        
      
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

        if(context.performed)
        {
            Golpe();
            
            
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
       
        
        if (!isAttacking && !isSecondAttack)
        {
            animator.SetTrigger("Golpe");
            isAttacking = true;
            rigidBody.velocity = Vector3.zero;
            canMove = false;
           
        }
        else if(isAttacking && timeNextAttack > 0 && !isSecondAttack)
        {
            animator.SetTrigger("Attack2");
            isAttacking = true;
            timeNextAttack = timeBetweenAttacks;
            isSecondAttack = true;
            rigidBody.velocity = Vector3.zero;
            canMove = false;
        }
        else if(isSecondAttack && timeLastAttack > 0)
        {
            timeNextAttack = timeBetweenAttacks;
            animator.SetTrigger("Attack3");
            //isSecondAttack = false;
            //isAttacking = false;
            //timeLastAttack = timeBetweenAttacks;
            rigidBody.velocity = Vector3.zero;
            canMove = false;
            StartCoroutine(FinishCombo(timeFinishCombo));
        }
        

        Collider2D[] objects = Physics2D.OverlapCircleAll(attackManager.position, areaAttack);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<Enemy>().TakeDamage(damageAttack);
            }
        }
        
        
    }
    private IEnumerator FinishCombo(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        isSecondAttack = false;
        isAttacking = false;
        timeLastAttack = timeBetweenAttacks;
        Invoke("RecoverMovement", duration);
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
        
        if (canRoll && grounded && !isAirAttack && !isPickUp && !canAttack)
        {
            StartCoroutine(Roll());
        }
    }

    /// <summary>
    /// Corrutina para la animacion de Roll
    /// </summary>
    /// <returns></returns>
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
        if(collision.tag == "DamageZone" && !isImmune && !isInvincible)
        {
            Dead();
        }
        if (collision.CompareTag("PickUpHealth"))
        {
            animator.SetTrigger("Health");
            PickUp();
        }
        else if(collision.CompareTag("PickUpPower"))
        {
            animator.SetTrigger("Power");
            PickUp();
        }
    }
    void PickUp()
    {
        rigidBody.velocity = Vector3.zero;
        canMove=false;
       
        isPickUp = true;
        
        Invoke("RecoverMovement", 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible || (canRoll && collision.collider.CompareTag("Enemy")))
        {
            return;
        }
       else if (collision.collider.CompareTag("Enemy") && !isInvincible && !isImmune)
        {
            //indicamos al jugador que muera
            Dead();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //si el objetos colisionado es un enemigo 

        if (collision.tag == "Enemy" && !isInvincible && !isImmune)
        {
            //indicamos al jugador que muera
            Dead();
        }
    }

   
    /// <summary>
    /// Metodo para girar el spirte del player
    /// </summary>
    private void Flip()
    {
        isFancingRight = !isFancingRight;
        Vector3 localScale  =transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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

    /// <summary>
    /// Metodo para el salto
    /// </summary>
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
    /// <summary>
    /// Metodo para el ataque desde arriba
    /// </summary>
    void AirAttack()
    {
        if (!grounded)
        {
            isAirAttack = true;
            canMove = false;
            
            
            animator.SetTrigger("AirAttack");
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);

            Collider2D[] objects = Physics2D.OverlapCircleAll(airAttackManager.position, airAreaAttack);

            foreach (Collider2D collider in objects)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.transform.GetComponent<Enemy>().TakeDamage(airDamageAttack);
                    Debug.Log("Le ha dado el airAttacck");
                }
            }
            Invoke("RecoverMovement", 1);
        }
        
      
       
    }


    /// <summary>
    /// Metodo chekear el suelo
    /// </summary>
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

   
    /// <summary>
    /// Metodo que almacena los parametros de las animaciones
    /// </summary>
    private void FeedAnimation()
    {
        //indicamos al animator cuando el player se encuentra en contacto con el suelo
        animator.SetBool("Grounded", grounded);
        //le transmito el valor absoluto de la velocidad en el eje x
        animator.SetFloat("Velocity", Mathf.Abs(rigidBody.velocity.x));
        
        animator.SetBool("Roll", !canRoll);
        animator.SetBool("Attack", !canAttack);
       
        if (Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon)
        {
            animator.SetFloat("VelocityY", Mathf.Sign(rigidBody.velocity.y));
        }
        else
        {
            animator.SetFloat("VelocityY", 0);
        }




    }

    /// <summary>
    /// Metodo para la muerte del player
    /// </summary>
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

    /// <summary>
    /// Metodo con el que el player recibe daño del enemigo
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(float dmg)
    {
        if (!isInvincible && !isImmune)
        {
            playerHealth -= dmg;
            healthBar.ChangedCurrentHealth(playerHealth);
            animator.SetTrigger("Hurt");
        }
        

        if (playerHealth <= 0) 
        {
            Dead();
        }
    }

    /// <summary>
    /// Metodo con el que el player se cura vida
    /// </summary>
    /// <param name="restoredHealth"></param>
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

    /// <summary>
    /// Metodo con el que player puede subir las escaleras
    /// </summary>
    void Climb()
    {
        if((input.y !=0 || isClimbing) && (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Stairs"))))
        {
            Vector2 upSpeed = new Vector2(rigidBody.velocity.x, input.y * climbSpeed);
            rigidBody.velocity = upSpeed;
            rigidBody.gravityScale = 0;
            isClimbing = true;
            canJump = false;
        }
        else
        {
            rigidBody.gravityScale = initialGravity;
            isClimbing=false;
        }
        if (grounded)
        {
            isClimbing = false;
            canJump = true;
        }
        animator.SetBool("isClimbing", isClimbing);
    }

     void RecoverMovement()
    {
        canMove = true;
        isAirAttack = false;
        isPickUp = false;
        canAttack = false;
    }

    /// <summary>
    /// Contador de timepo para controlar la duracion del powerUp
    /// </summary>
    /// <returns></returns>
    private IEnumerator PowerUpTime(float duration)
    {
        damageAttack += 10;
        //Inicializamos el contador  de tiempo
        powerUpCounter = duration;
        //mientras el contador el timepo transcurrido desde el ultimo frame
        while (powerUpCounter > 0)
        {
            powerUpCounter -= Time.deltaTime;
            //no hacemos ninguna espera
            yield return null;
            
        }
        if(powerUpCounter <= 0)
        {
            damageAttack -= 10;
        }
    }

    /// <summary>
    /// Activa el PowerUp
    /// </summary>
    public void ActivatePowerUp(float duration)
    {
        
        //si extiste la corrutina la detenemos
        if (powerUpCoroutine != null) StopCoroutine(powerUpCoroutine);
        //inicimaos la corrutina nuevamente
        powerUpCoroutine = StartCoroutine(PowerUpTime(duration));
    }

    //-------------------------------------- Menu de Chetos --------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    public void SetImmunity()
    {
        isImmune = !isImmune;
    }

    public void MoreDamage()
    {
        moreDamage = !moreDamage;
        if (moreDamage)
        {
            damageAttack *= 999;
        }
        else
        {
            damageAttack /= 999;
        }
        
    }
    public void MoreJumpForce()
    {
        moreJumpForce=!moreJumpForce;
        if(moreJumpForce)
        {
            jumpForce *= 2;
        }
        else
        {
            jumpForce /= 2;
        }
    }
}

