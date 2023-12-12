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

   public void ChangedMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void ChangedCurrentHealth(float currentHealth)
    {
        slider.value = currentHealth;
    }

    public void InitializeHealthBar( float currentHealth)
    {
        ChangedMaxHealth(currentHealth);
        ChangedCurrentHealth(currentHealth);
    }
}
