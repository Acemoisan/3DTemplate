using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMagic : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Animator playerAnimation;
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] PlayerControllerSO playerControllerSO;
    [SerializeField] HUDUI hudUI;
    [SerializeField] InventoryUI1 inventoryUI1;
    [SerializeField] HUDMessage HUDMessage;
    public List<SpellSO> spells;
    [SerializeField] Transform handPositionAttackPoint;
    [SerializeField] Camera virtualCamera;
    [SerializeField] GameObject leftHandIcon;
    [SerializeField] GameObject rightHandIcon;
    [SerializeField] List<AudioClip> projectileAudioClips;
    [SerializeField] PlaySFX playSFX;



    [Header("Config")]
    [SerializeField] float projectileEnergyCost;
    [SerializeField] float auraEnergyCost;



    [Header("Events")]
    public UnityEvent OnProjectileShoot;
    public UnityEvent OnAuraSpawn;



    //Private
    SpellSO leftHandSpell;
    SpellSO rightHandSpell;
    int currentRightHandSpellIndex;
    int currentLeftHandSpellIndex;
    GameObject leftHandSpellPrefab;
    GameObject rightHandSpellPrefab;
    GameObject auraPrefab;




    void Start()
    {
        currentRightHandSpellIndex = 0;
        currentLeftHandSpellIndex = 0;
        rightHandSpell = spells[currentRightHandSpellIndex];
        leftHandSpell = spells[currentLeftHandSpellIndex];
        leftHandIcon.SetActive(false);
        rightHandIcon.SetActive(false);
        //List<ItemSO> itemSOs = spells.Cast<ItemSO>().ToList();        
        inventoryUI1.ShowAllSpellsOnButtons();
    }

    public void AddNewSpell(SpellSO spell)
    {
        if(spells.Contains(spell)) { playerAttributes.IncreaseEnergy(50); return; }
        if(HUDMessage == null) { Debug.LogError("No HUDMessage set"); return; }
        HUDMessage.TriggerPopUpTextTemporary("New Spell! ~ " + spell.GetItemName() + " ~");
        spells.Add(spell);
        inventoryUI1.ShowAllSpellsOnButtons();
    }


    public void NextRighHandSpell(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            currentRightHandSpellIndex++;
            if(currentRightHandSpellIndex >= spells.Count)
            {
                currentRightHandSpellIndex = 0;
            }
            rightHandSpell = spells[currentRightHandSpellIndex];
            hudUI.HighlightButton(currentRightHandSpellIndex);
            leftHandIcon.SetActive(false);
            rightHandIcon.SetActive(true);
        }
    }

    public void NextLeftHandSpell(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            currentLeftHandSpellIndex++;
            if(currentLeftHandSpellIndex >= spells.Count)
            {
                currentLeftHandSpellIndex = 0;
            }
            leftHandSpell = spells[currentLeftHandSpellIndex];
            hudUI.HighlightButton(currentLeftHandSpellIndex);
            leftHandIcon.SetActive(true);
            rightHandIcon.SetActive(false);
        }
    }


    public void LeftHandSpell(InputAction.CallbackContext value)
    {
        //the idea, is to get the current left hand spell, and then set the prefab of that spell.
        //I then play an animation, corresponding to the spell type, and the Instantiation EVENT is called from anim. spawning the set spellPrefab
        if (value.started)
        {
            if(leftHandSpell == null) { return; }
            if(leftHandSpell.spellType == SpellType.Projectile)
            {
                if(playerAttributes.GetPlayerEnergy() < projectileEnergyCost) { return; }
                playerAttributes.ReduceEnergy(projectileEnergyCost);

                //play left hand projectile anim
                playerAnimation.SetTrigger("LeftHandSpell");
                leftHandSpellPrefab = leftHandSpell.spellPrefab;
                OnProjectileShoot?.Invoke(); //screenshake?
            }
            else if(leftHandSpell.spellType == SpellType.Aura)
            {
                if(playerAttributes.GetPlayerEnergy() < auraEnergyCost) { return; }
                playerAttributes.ReduceEnergy(auraEnergyCost);

                auraPrefab = leftHandSpell.spellPrefab;
                //play aura anim. from event. because animation uses both hands.
                OnAuraSpawn?.Invoke();
            }
            else if(leftHandSpell.spellType == SpellType.SpeedBoost)
            {
                if(playerAttributes.GetPlayerEnergy() < auraEnergyCost) { return; }
                playerAttributes.ReduceEnergy(auraEnergyCost);

                auraPrefab = leftHandSpell.spellPrefab;
                //play aura anim. from event. because animation uses both hands.
                OnAuraSpawn?.Invoke();
                StartCoroutine(MoveSpeedBoost());
            }
            leftHandIcon.SetActive(true);
            rightHandIcon.SetActive(false);
            hudUI.HighlightButton(currentLeftHandSpellIndex);
        }
    }

    public void RightHandSpell(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if(rightHandSpell == null) { return; }
            if(rightHandSpell.spellType == SpellType.Projectile)
            {
                if(playerAttributes.GetPlayerEnergy() < projectileEnergyCost) { return; }
                playerAttributes.ReduceEnergy(projectileEnergyCost);

                //play right hand projectile anim
                playerAnimation.SetTrigger("RightHandSpell");
                rightHandSpellPrefab = rightHandSpell.spellPrefab;
                OnProjectileShoot?.Invoke(); //extra effects
            }
            else if(rightHandSpell.spellType == SpellType.Aura)
            {
                if(playerAttributes.GetPlayerEnergy() < auraEnergyCost) { return; }
                playerAttributes.ReduceEnergy(auraEnergyCost);


                auraPrefab = rightHandSpell.spellPrefab;
                //play aura anim. from event. because animation uses both hands.
                OnAuraSpawn?.Invoke();
            }
            else if(rightHandSpell.spellType == SpellType.SpeedBoost)
            {
                if(playerAttributes.GetPlayerEnergy() < auraEnergyCost) { return; }
                playerAttributes.ReduceEnergy(auraEnergyCost);

                auraPrefab = rightHandSpell.spellPrefab;
                //play aura anim. from event. because animation uses both hands.
                OnAuraSpawn?.Invoke();
                StartCoroutine(MoveSpeedBoost());
            }
            leftHandIcon.SetActive(false);
            rightHandIcon.SetActive(true);
            hudUI.HighlightButton(currentRightHandSpellIndex);
        }
    }

    IEnumerator MoveSpeedBoost()
    {
        playerControllerSO.IncreaseMoveSpeed(2f);
        yield return new WaitForSeconds(5f);
        playerControllerSO.DefaultMoveSpeed();
    }

    public void SpawnMagicAura()
    {
        if(auraPrefab == null) { Debug.LogError("No aura prefab set"); return; }
        GameObject magicAura = Instantiate(auraPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(magicAura, 5f);
    }

    public void SpawnProjectileLeftHand()
    {
        if(leftHandSpellPrefab == null) { Debug.LogError("No spell prefab set for left hand"); return; }
        GameObject projectile = Instantiate(leftHandSpellPrefab, handPositionAttackPoint.position, Quaternion.identity) as GameObject;
        SetupProjectile(projectile);
    }

    public void SpawnProjectileRightHand()
    {
        if(rightHandSpellPrefab == null) { Debug.LogError("No spell prefab set for right hand"); return; }
        GameObject projectile = Instantiate(rightHandSpellPrefab, handPositionAttackPoint.position, Quaternion.identity) as GameObject;
        SetupProjectile(projectile);
    }

    public void SetupProjectile(GameObject projectile)
    {
        if(projectile.GetComponent<SpellProjectile>() != null)
        {
            Vector3 shootPosition = virtualCamera.transform.forward;
            projectile.GetComponent<SpellProjectile>().SetDirection(shootPosition);
        }
    }

    public void PlayRandomAudioClip()
    {
        if(projectileAudioClips.Count == 0) { return; }
        playSFX.PlayClip(projectileAudioClips[Random.Range(0, projectileAudioClips.Count)]);
    }
}
