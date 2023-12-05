using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyHealth;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        Destroy(gameObject);
    }

}
