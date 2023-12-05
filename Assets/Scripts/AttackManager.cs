using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private Transform attackManager;
    [SerializeField]
    private float areaAttack;
    [SerializeField]
    private float damageAttack;
    private PlayerController playerData;


    private void Start()
    {
        playerData.animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void InAttack(InputAction.CallbackContext context)
    {
        
    }

    private void Attack()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(attackManager.position, areaAttack);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<Enemy>().TakeDamage(damageAttack);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackManager.position, areaAttack);
    }



}
