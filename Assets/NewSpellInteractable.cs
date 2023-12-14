using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpellInteractable : Interactable
{
    [SerializeField] SpellSO spellSO;


    public void GiveSpellToPlayer()
    {
        if(GetPlayer().GetComponentInChildren<PlayerMagic>() == null) { return; }
        PlayerMagic playerMagic = GetPlayer().GetComponentInChildren<PlayerMagic>();
        playerMagic.AddNewSpell(spellSO);
        Destroy(gameObject);
    }
}
