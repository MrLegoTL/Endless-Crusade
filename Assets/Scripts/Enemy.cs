using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyHealth;
    private Animator animator;
    private BoxCollider2D enemyCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
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

}
