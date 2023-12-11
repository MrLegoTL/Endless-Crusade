using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyHealth;
    private Animator animator;
    private BoxCollider2D enemyCollider;
    [SerializeField]
    public bool isHurt=false;

    [SerializeField]
    private Transform attackManager;
    [SerializeField]
    private float areaAttack;
    [SerializeField]
    private float damageEnemyAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
      
        enemyHealth -= dmg;
        if(enemyHealth <= 0 ) 
        {
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
       animator.SetTrigger("DeathEnemy");

        DisableColliderEnemy();
    }

    void DisableColliderEnemy()
    {
        if(enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
    }

   void EnemyHurt()
    {
        animator.SetTrigger("Hurt");
       
        
    }

    void EnemyAttack()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(attackManager.position, areaAttack);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                collider.transform.GetComponent<PlayerController>().TakeDamage(damageEnemyAttack);

            }
        }
    }

    private void OnDrawGizmos()
    {
     
        //Gizmos para areaAttack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackManager.position, areaAttack);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
        }
    }

}
