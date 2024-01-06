using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Interactable
{

    [SerializeField] float healthPerSecond;


    public void HealPlayer()
    {
        StartCoroutine(HealPlayerRoutine());
    }

    private bool isHealing = false; // Flag to control the healing loop
    IEnumerator HealPlayerRoutine()
    {
        isHealing = true;
        while(isHealing)
        {
            Debug.Log("Healing player");
            PlayerAttributes playerAttributes = GetPlayer().GetComponentInChildren<PlayerAttributes>();
            playerAttributes.IncreaseHealth(healthPerSecond);
            yield return new WaitForSeconds(1f);
        }
    }

    public void StopHealing()
    {
        Debug.Log("Stop healing");
        isHealing = false;
    }
}
