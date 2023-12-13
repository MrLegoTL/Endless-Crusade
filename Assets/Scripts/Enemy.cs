using System.Collections;
using System.Collections.Generic;
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

    [Header("Enemy Attack")]
    [SerializeField]
    private Transform attackManager;
    [SerializeField]
    private float attackArea;
    [SerializeField]
    private float attackDamage;
    public float playerDistance;
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
          playerDistance = Vector2.Distance(transform.position, player.position);
        animator.SetFloat("PlayerDistance", playerDistance);
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
    /// Metodo con el que el enemigo recibe daño del player
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(float dmg)
    {
      if(enemyHealth > 0)
        {
         enemyHealth -= dmg;
            if(animator != null)
            {
                animator.SetTrigger("Hurt");
            }

        if(enemyHealth <= 0 ) 
        {
            EnemyDeath();
        }
       }
    }

    /// <summary>
    /// Metodo para la muerte del enemigo
    /// </summary>
    void EnemyDeath()
    {
        if(animator != null)
        {
            animator.SetBool("isDead", true);
        }
       

        DisableColliderEnemy();
    }
    /// <summary>
    /// Metoddo para desactivar el collider del enemigo cuando muere
    /// </summary>
    void DisableColliderEnemy()
    {
        if(enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        animator.SetTrigger("Attack");
    //    }
    //}


}
