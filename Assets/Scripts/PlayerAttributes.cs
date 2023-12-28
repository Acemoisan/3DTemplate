using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField] StatBar energyStatBar;
    [SerializeField] PlayerControllerSO _playerControllerSO;


    


    [Header("Entity Stats")]
    string _playerName;
    int _playerLevel;


    [Header("Health")]
    [SerializeField] float _playerHealth;
    [SerializeField] HealthBarManager healthBar;


    [Header("Energy")]
    [SerializeField] float _playerEnergy;
    [SerializeField] float energyPerSecond;
    [SerializeField] bool autoRegen;



    [Header("Misc")]
    [SerializeField] float _playerDefense;


    public UnityEvent OnHeal;
    public UnityEvent EnergyIncreaseEvent;
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeathEvent;

    float _originalHealth;
    float _originalEnergy;




    void Start()
    {
       UpdateStatBars();
       UpdateHealthBar();
       SetPlayerName("Bob");


       if(autoRegen)    
       {
            EnergyRegenTimed(true, energyPerSecond, 1000);
       }

        _originalHealth = _playerHealth;
        _originalEnergy = _playerEnergy;
    }




    #region ENERGY ==============================
    public void IncreaseEnergy(float value)
    {
        _playerEnergy += value;

        if (_playerEnergy > GetPlayerMaxEnergy())
        {
            _playerEnergy = GetPlayerMaxEnergy();
        }
        else if (_playerEnergy < 0)
        {
            _playerEnergy = 0;
        }

        EnergyIncreaseEvent.Invoke();
        UpdateStatBars();
    }

    public void ReduceEnergy(float value)
    {
        _playerEnergy -= value;

        if (_playerEnergy < 0)
        {
            _playerEnergy = 0;
        }

        UpdateStatBars();
    }




    float energyRegenTime;
    bool regenerating = false;
    int regenCountIndex = 0;


    public void EnergyRegenTimed(bool regen, float energyPerSecond, float regenTime)
    {
        if(regen == false)
        {
            regenerating = false;
            StopCoroutine(RegenerateEnergyTimed());
            return;
        }

        this.energyPerSecond = energyPerSecond;
        this.energyRegenTime = regenTime;
        if (regenerating) return;
        
        regenerating = true;
        StopCoroutine(RegenerateEnergyTimed());
        StartCoroutine(RegenerateEnergyTimed());
    }


    public IEnumerator RegenerateEnergyTimed()
    {
        regenCountIndex = 0;

        while(regenerating)
        {
            regenCountIndex++;

            _playerEnergy += energyPerSecond;

            UpdateStatBars();

            if (_playerEnergy > GetPlayerMaxEnergy())
            {
                _playerEnergy = GetPlayerMaxEnergy();
            }

            if (regenCountIndex >= energyRegenTime)
            {
                regenerating = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }    
    #endregion





    #region HEALTH ==============================
    public void IncreaseHealth(float value)
    {
        ChangeHealth(value);
            
        UpdateHealthBar();
    }

    public void DecreaseHealth(float damage)
    {
        //if(_playerController.IsDodging()) { return; }

        float damageToDeal = damage - GetPlayerDefense();

        ChangeHealth(-damageToDeal);

        if(OnHitEvent != null) { OnHitEvent.Invoke(); }
        

        if (_playerHealth <= 0)
        {
            Kill();
        }
    }
    void ChangeHealth(float value)
    {
        _playerHealth += value;

        if (_playerHealth > GetPlayerMaxHealth())
        {
            _playerHealth = GetPlayerMaxHealth();
        }
        else if (_playerHealth < 0)
        {
            _playerHealth = 0;
        }
            
        UpdateHealthBar();
    }

    public void Kill()
    {
        _playerHealth = 0;
        if(OnDeathEvent != null) OnDeathEvent.Invoke();
    }


    public void HealPlayerAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnHeal?.Invoke();
        }
    }

    float healthPerSecond;
    float healthRegenTime;
    bool regeneratingHealth = false;

    public void StartHealthRegen(float healthPerSecond, float regenTime)
    {
        this.healthPerSecond = healthPerSecond;
        this.healthRegenTime = regenTime;
        if (regeneratingHealth) return;
        InvokeRepeating("RegenerateHealth", 1f, 1f);
    }

    int healthRegenCountIndex = 0;
    public void RegenerateHealth()
    {
        regeneratingHealth = true;
        healthRegenCountIndex++;

        _playerHealth += healthPerSecond;

        if (_playerHealth > GetPlayerMaxHealth())
        {
            _playerHealth = GetPlayerMaxHealth();
        }

        UpdateStatBars();

        if (healthRegenCountIndex >= healthRegenTime)
        {
            regeneratingHealth = false;
            CancelInvoke("RegenerateHealth");
            healthRegenCountIndex = 0;
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.UpdateHealthBars(GetPlayerHealth(), GetPlayerMaxHealth());
    }
    #endregion





    #region SPECIAL SKILLS ==============================
    public void TemporarilyIncreaseDefense(float value, float resistTime)
    {
        _playerDefense += value;
        Invoke("ResetDefense", resistTime);
    }

    void ResetDefense()
    {
        _playerDefense = 0;
    }
    #endregion





    public void RefreshStats()
    {
        _playerEnergy = GetPlayerMaxEnergy();
        _playerHealth = GetPlayerMaxHealth();
        UpdateStatBars();
    }

    public void RevertStats()
    {
        _playerEnergy = _originalEnergy;
        _playerHealth = _originalHealth;
        UpdateStatBars();
    }

    void UpdateStatBars()
    {
        UpdateEnergyStatBarValue();

    }
    
    public void UpdateEnergyStatBarValue()
    {
        energyStatBar.UpdateSliderBarValue(GetPlayerMaxEnergy(), GetPlayerEnergy());
    }









    #region GETTERS && SETTERS ==============================
    public void SetPlayerName(string name)
    {
        _playerName = name;
    }
    public string GetPlayerName()
    {
        return _playerName;
    }



    public void SetPlayerLevel(int level)
    {
        _playerLevel = level;
    }
    public int GetPlayerLevel()
    {
        return _playerLevel;
    }



    public void SetPlayerHealth(float health)
    {
        _playerHealth = health;
        if(_playerHealth > GetPlayerMaxHealth())
        {
            _playerHealth = GetPlayerMaxHealth();
        }
        else if(_playerHealth < 0)
        {
            _playerHealth = 0;
        }
    }
    public float GetPlayerHealth()
    {
        return _playerHealth;
    }
    public float GetPlayerMaxHealth()
    {
        return _playerControllerSO._playerMaxHealth;
    }



    public void SetPlayerEnergy(float energy)
    {
        _playerEnergy = energy;
        if (_playerEnergy > GetPlayerMaxEnergy())
        {
            _playerEnergy = GetPlayerMaxEnergy();
        }
        else if (_playerEnergy < 0)
        {
            _playerEnergy = 0;
        }
    }
    public float GetPlayerEnergy()
    {
        return _playerEnergy;
    }
    public float GetPlayerMaxEnergy()
    {
        return _playerControllerSO._playerMaxEnergy;
    }





    public void SetPlayerDefense(float defense)
    {
        _playerDefense = defense;
    }
    public float GetPlayerDefense()
    {
        return _playerDefense;
    }
    #endregion
}
