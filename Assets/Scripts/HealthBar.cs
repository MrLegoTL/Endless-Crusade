using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    /// <summary>
    /// Establece el valor máximo de salud para la barra de salud.
    /// </summary>
    /// <param name="maxHealth"></param>
    public void ChangedMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    /// <summary>
    /// Establece el valor actual de salud para la barra de salud.
    /// </summary>
    /// <param name="currentHealth"></param>
    public void ChangedCurrentHealth(float currentHealth)
    {
        slider.value = currentHealth;
    }
    /// <summary>
    /// Inicializa la barra de salud con valores máximos y actuales.
    /// </summary>
    /// <param name="currentHealth"></param>
    public void InitializeHealthBar( float currentHealth)
    {
        ChangedMaxHealth(currentHealth);
        ChangedCurrentHealth(currentHealth);
    }
}
