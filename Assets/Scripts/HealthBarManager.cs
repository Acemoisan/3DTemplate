using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [Header("CHOOSE ONE")]
    [SerializeField] bool sliderToggle;
    [SerializeField] bool heartsToggle;

    [Header("Dependencies")]
    [SerializeField] GameObject _healthBarSliderObject;
    [SerializeField] GameObject _healthBarHeartHolder;
    [SerializeField] StatBar _healthBarSlider;
    [SerializeField] List<GameObject> hearts;


    void Start()
    {
        if(sliderToggle == false && heartsToggle == false) { Debug.LogError("You must choose either sliderToggle or heartsToggle on " + this); }

        if(sliderToggle)
        {
            _healthBarSliderObject.SetActive(true);
            _healthBarHeartHolder.SetActive(false);
        }
        else if(heartsToggle)
        {
            _healthBarSliderObject.SetActive(false);
            _healthBarHeartHolder.SetActive(true);
        }
    }
    
    public void UpdateHealthBars(float currentHealth, float maxHealth)
    {
        if(sliderToggle)
        {
            UpdateSliderBar(currentHealth, maxHealth);
        }
        else if(heartsToggle)
        {
            UpdateHearts(currentHealth, maxHealth);
        }
    }


    private void UpdateSliderBar(float currentHealth, float maxHealth)
    {
        _healthBarSlider.UpdateSliderBarValue(maxHealth, currentHealth);
    }
    
    
    private void UpdateHearts(float currentHealth, float maxHealth)
    {
        float healthPerHeart = maxHealth / hearts.Count;

        int fullHearts = Mathf.FloorToInt(currentHealth / healthPerHeart);

        // Activate the full hearts
        for (int i = 0; i < fullHearts; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }

        // Deactivate the empty hearts
        for (int i = fullHearts; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }
}
