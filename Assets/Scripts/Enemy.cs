using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField]
    private float enemyHealth;
    private Animator animator;
    public Rigidbody2D rb2D;
    [SerializeField]
    private LayerMask playerLayer;    
    private BoxCollider2D enemyCollider;
    [SerializeField]
    public bool isHurt=false;
    [SerializeField]
    private bool isInvincible=false;
    [SerializeField]
    private float timeInvincible;
    [SerializeField]
    private int enemyDeathCount = 1;
    [SerializeField]
    private float hitForce = 1f;

    [Header("Enemy Attack")]
    [SerializeField]
    private Transform attackManager;
    [SerializeField]
    private float attackArea;
    [SerializeField]
    private float attackDamage;
    public float playerDistance;
    public float playerDistanceX;
    public float playerDistanceY;
    [Header("Enemy Second Attack")]
    [SerializeField]
    private Transform secondAttackManager;
    [SerializeField]
    private float secondAttackArea;
    [SerializeField]
    private float secondAttackDamage;
    Transform player;
    private bool seeRight = true;

    private void Start()
    {
       
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform;
        

       
    }
    private void Update()
    {
        playerDistanceY = Mathf.Abs(transform.position.y - player.position.y);
        playerDistanceX = Mathf.Abs(transform.position.x - player.position.x);
        //playerDistance = Vector2.Distance(transform.position, player.position);
        animator.SetFloat("PlayerDistance", playerDistanceX);
        animator.SetFloat("PlayerDistanceY", playerDistanceY);
      
    }
    
    /// <summary>
    /// Metodo para el ataque del enemigo
    /// </summary>
    void EnemyAttack()
    {
       
            //animator.SetTrigger("Attack");

            Collider2D[] objects = Physics2D.OverlapCircleAll(attackManager.position, attackArea);
        foreach (Collider2D collision  in objects)
        {
            
            if (collision.CompareTag("Player")) 
            {
                collision.GetComponent<PlayerController>().TakeDamage(attackDamage);
                
            }
        }


    }
    /// <summary>
    /// Metodo para el segundo ataque del enemigo
    /// </summary>
    void EnemySecondAttack()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(secondAttackManager.position, secondAttackArea);
        foreach (Collider2D collision in objects)
        {

            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().TakeDamage(secondAttackDamage);

            }
        }
    }

    /// <summary>
    /// Metodo con el que el enemigo recibe da�o del player
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(float dmg)
    {
      if(enemyHealth > 0 && !isInvincible)
        {
         enemyHealth -= dmg;
            if(animator != null)
            {
                animator.SetTrigger("Hurt");
                //StartCoroutine(SetInvincible());
            }

        if(enemyHealth <= 0 ) 
        {
                if (animator != null)
                {
                    GameManager.instance.EnemyCount(enemyDeathCount);
                    animator.SetBool("isDead", true);
                }
        }
            if (rb2D != null)
            {
                float direction = GetDirectionFromHit();
                rb2D.velocity= new Vector2(-direction * hitForce, rb2D.velocity.y);
            }
      }
    }
    /// <summary>
    /// M�todo para obtener la direcci�n desde la que se realiz� el ataque
    /// </summary>
    /// <returns></returns>
    float GetDirectionFromHit()
    {
        float direction = 0f;

        // Comparar las posiciones para determinar la direcci�n del ataque
        if (player.position.x > transform.position.x)
        {
            // Golpe desde la derecha, desplazar hacia la izquierda
            direction = 1f;
        }
        else
        {
            // Golpe desde la izquierda, desplazar hacia la derecha
            direction = -1f;
        }

        return direction;
    }

    /// <summary>
    /// Metodo para la muerte del enemigo
    /// </summary>
    void EnemyDeath()
    {
        
        Destroy(gameObject);
        

    }
    
    /// <summary>
    /// Metodo con el que el enemigo mira al player
    /// </summary>
    public void SeePlayer()
    {
        if((player.position.x > transform.position.x && !seeRight) || (player.position.x < transform.position.x && seeRight))
        {
            seeRight = !seeRight;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }


    private void OnDrawGizmos()
    {

        //Gizmos para areaAttack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackManager.position, attackArea);
        //Gizmos para secondareaAttack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(secondAttackManager.position, secondAttackArea);
    }

    


}
