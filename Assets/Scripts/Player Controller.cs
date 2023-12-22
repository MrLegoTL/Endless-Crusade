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

    [Header("FeedBack")]
    private SpriteRenderer playerSprite;
    [Header("PowerUp")]
    //para realizar el contador inteno de la duración del powerup
    public float powerUpCounter = 0f;
    public float immunityPowerUpCounter;
    public float soulPowerUpCounter;
    //corrutina para gestionar la duración del powerUp
    private Coroutine powerUpCoroutine;
    private Coroutine immunityCoroutine;
    private Coroutine soulCoroutine;
    public ParticleSystem PowerParticles;
    

    //referencia al animator
    public Animator animator;
    public bool isDead = false;
    public bool isPickUp = false;
    // Start is called before the first frame update

    void Start()
    {
        playerSprite = GetComponentInChildren<SpriteRenderer>();
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
    /// <summary>
    /// Metodo que activa activa la accion de moverse mediante el InputSystem
    /// </summary>
    /// <param name="context"></param>
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
    /// <summary>
    /// Metodo que activa activa la accion de atacar mediante el InputSystem
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {

        if(context.performed)
        {
            Golpe();
            
            
        }                             
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
    /// <summary>
    /// Corrutina para Terminar el combo de ataque
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator FinishCombo(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        isSecondAttack = false;
        isAttacking = false;
        timeLastAttack = timeBetweenAttacks;
        Invoke("RecoverMovement", duration);
    }
    /// <summary>
    /// Metodo que activa activa la accion de Roll mediante el InputSystem
    /// </summary>
    /// <param name="context"></param>
    public void OnDash(InputAction.CallbackContext context)
    {
        
        if (canRoll && grounded && !isAirAttack && !isPickUp )
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
        else if (collision.CompareTag("PickUpImmunity"))
        {
            animator.SetTrigger("Immunity");
            PickUp();
        }
    }
    /// <summary>
    /// Metodo para cuando el player coga un Item no se puesdar mover
    /// </summary>
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

    /// <summary>
    /// Metodo que activa activa la accion de saltar mediante el InputSystem
    /// </summary>
    /// <param name="context"></param>
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
    /// <summary>
    /// Metodo que activa activa la accion de ataque desde arriba mediante el InputSystem
    /// </summary>
    /// <param name="context"></param>
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
        Invoke("EndGame", 1f);
    }
    /// <summary>
    /// Metodo que llama a la funcion de terminar partida de Game Manager
    /// </summary>
    void EndGame()
    {
        GameManager.instance.EndGame();
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
    /// <summary>
    /// Metdodo para que cuando se activa se pueda hacer otros movimientos despues de algunas animaciones
    /// </summary>
     void RecoverMovement()
    {
        canMove = true;
        isAirAttack = false;
        isPickUp = false;
        canAttack = false;
    }

    /// <summary>
    /// Contador de timepo para controlar la duracion del powerUp de daño
    /// </summary>
    /// <returns></returns>
    private IEnumerator PowerUpTime(float duration)
    {
        damageAttack += 10;
        PostPorcessingManager posProcess = FindObjectOfType<PostPorcessingManager>();
        if(posProcess != null)
        {
            posProcess.ActivePowerPostProcess();
        }
        PowerParticles.startColor = new Color(0.44f, 0, 1, 1);
        PowerParticles.Play();
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
            PowerParticles.Stop();
            if(posProcess != null)
            {
                posProcess.DesactivePowerPostProcess();
            }
        }
    }

    /// <summary>
    /// Activa el PowerUp de daño
    /// </summary>
    public void ActivatePowerUp(float duration)
    {
        
        //si extiste la corrutina la detenemos
        if (powerUpCoroutine != null) StopCoroutine(powerUpCoroutine);
        //inicimaos la corrutina nuevamente
        powerUpCoroutine = StartCoroutine(PowerUpTime(duration));
        Debug.Log("Seha activado el Power");
    }

    /// <summary>
    /// Contador de timepo para controlar la duracion del powerUp de inmunidad
    /// </summary>
    /// <returns></returns>
    private IEnumerator ImmunityPowerUpTime(float timeImmunity)
    {
        isImmune = true;    
        PostPorcessingManager posProcess = FindObjectOfType<PostPorcessingManager>();
        if (posProcess != null)
        {
            posProcess.ActiveImmunityPostProcess();
        }
        PowerParticles.startColor = new Color(0,1,1,1);
        PowerParticles.Play();
        //Inicializamos el contador  de tiempo
        immunityPowerUpCounter = timeImmunity;
        //mientras el contador el timepo transcurrido desde el ultimo frame
        while (immunityPowerUpCounter > 0)
        {
            immunityPowerUpCounter -= Time.deltaTime;
            //no hacemos ninguna espera 
            yield return null;

        }
        if (immunityPowerUpCounter <= 0)
        {
            isImmune = false;            
            PowerParticles.Stop();
            if (posProcess != null)
            {
                posProcess.DesactiveImmunityPostProcess();
            }
        }
    }
  
    /// <summary>
    /// Activa el PowerUp de Inmunidad
    /// </summary>
    public void ActivateImmunityPowerUp(float timeImmunity)
    {

        //si extiste la corrutina la detenemos
        if (immunityCoroutine != null) StopCoroutine(immunityCoroutine);
        //inicimaos la corrutina nuevamente
        immunityCoroutine = StartCoroutine(ImmunityPowerUpTime(timeImmunity));
    }
    /// <summary>
    /// Corrutina para realizar los efectos para cuando obtiene este Power Up
    /// </summary>
    /// <param name="timeSoul"></param>
    /// <returns></returns>
    private IEnumerator SoulPowerUp(float timeSoul)
    {
        isImmune = true;
        jumpForce += 1;
        speed += 1;
        rollSpeed += 1;
        PostPorcessingManager posProcess = FindObjectOfType<PostPorcessingManager>();
        if (posProcess != null)
        {
            posProcess.ActiveSoulPostProcess();
        }
        
        //Inicializamos el contador  de tiempo
        soulPowerUpCounter = timeSoul;
        //mientras el contador el timepo transcurrido desde el ultimo frame
        while (soulPowerUpCounter > 0)
        {
            soulPowerUpCounter -= Time.deltaTime;
            //no hacemos ninguna espera 
            yield return null;

        }
        if (soulPowerUpCounter <= 0)
        {
            isImmune = false;
            speed -= 1;
            rollSpeed -= 1;
            jumpForce -= 1;
            if (posProcess != null)
            {
                posProcess.DesactiveSoulPostProcess();
            }
        }
        
    }
    /// <summary>
    /// Activa el PowerUp de Recogida de almas
    /// </summary>
    public void ActiveSoulPowerUp(float timeSoul)
    {
        //si extiste la corrutina la detenemos
        if (soulCoroutine != null) StopCoroutine(soulCoroutine);
        //inicimaos la corrutina nuevamente
        soulCoroutine = StartCoroutine(SoulPowerUp(timeSoul));
    }

    //-------------------------------------- Menu de Chetos --------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Metodo que te hace inmune
    /// </summary>
    public void SetImmunity()
    {
        isImmune = !isImmune;
    }
    /// <summary>
    /// Metodo que aumenta tu daño de ataque
    /// </summary>
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
    /// <summary>
    /// Metodod que aumenta tu fuerza de salto
    /// </summary>
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

