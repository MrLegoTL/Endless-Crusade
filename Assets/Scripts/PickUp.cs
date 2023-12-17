using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//cremaos un tipo de enumerado para especificar el tipo de coleccionable
public enum CollectableType { collectable, powerup, HealthGem}

//este script requerira que exita un componente de tipo AudioSource
[RequireComponent(typeof(AudioSource))]

public class PickUp : MonoBehaviour
{
    //especificamos el tipo de coleccionable, inicializandolo con el valor collectable
    public CollectableType type = CollectableType.collectable;
    [Header("PowerUp")]
    public float timeMoreDamage;
    [Header("Health Gem")]
    public float healthToRestore;
    [Header("SoulGem")]
    public int soulValue = 1;
    public AudioClip audioClip;
    public AudioSource audioSource;
    public Collider2D collider;
    public SpriteRenderer spriteRenderer;
     PlayerController player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case CollectableType.collectable:
                 GameManager.instance.PickupCollectable(soulValue);
                    break;
                case CollectableType.powerup:
                     player = collision.GetComponent<PlayerController>();
                    if (player != null) player.ActivatePowerUp(timeMoreDamage);
                    break;
                case CollectableType.HealthGem:
                    player = collision.GetComponent<PlayerController>();
                    if(player!=null)player.RestoredHealth(healthToRestore);
                    break;
                default:
                    break;
            }

           
            
            EffectSound();
            DeactivateCollactable();
        }
    }
    /// <summary>
    /// Metodo para realizar la asignacion de los componentes
    /// </summary>
    [ContextMenu("FillComponents")]
    public void FillComponents()
    {
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource.loop = false;   
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Desactiva el coleccionable a nivel interactivo y visual
    /// </summary>
    private void DeactivateCollactable()
    {
        //desactivamos toda posible interaccion entre jugador y coleccionable
        collider.enabled = false;
        //desactivamos el elementos visual del coleccionable
        spriteRenderer.enabled = false;
    }

    /// <summary>
    /// Metodo para el fecto de sonido
    /// </summary>
    public void EffectSound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    //private void OnTriggerEnter2D(Collider col)
    //{
    //    //Compruba si el item colisiona con algo con el tag Player
    //    if (col.CompareTag("Player"))
    //    {
    //        // el item es destruido
    //        Destroy(gameObject);
    //    }
    //}

}
