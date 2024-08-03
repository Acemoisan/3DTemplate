using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectType
{
    None,
    Slowness,
    Poison,
    Fire,
}


public class PlayerEffectsHandler : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] VFXController vfxController;
    public EffectType currentEffect = EffectType.None;
    public float remainingDuration;
    [SerializeField] List<UnityEngine.GameObject> effects;



    public void AddEffect(EffectType effectType, float duration)
    {
        currentEffect = effectType;

        switch (effectType)
        {
            case EffectType.None:
                ClearEffects();
                break;
            case EffectType.Slowness:
                StartCoroutine(Slowness(duration));
                break;
            case EffectType.Poison:
                StartCoroutine(Poison(duration));
                break;
        }
    }

    public void ClearEffects()
    {
        remainingDuration = 1;
        currentEffect = EffectType.None;
        StopAllCoroutines();
    }

    IEnumerator Slowness(float duration)
    {
        //PARTICLE EFFECT
        SpawnParticleEffect(effects[1]);
        

        float cachedSpeed = playerController.WalkSpeed;
        remainingDuration = duration;
        cachedSpeed = playerController.WalkSpeed;
        float slowedMoveSpeed = playerController.WalkSpeed *.3f;
        playerController.SetMoveSpeed(slowedMoveSpeed);

        
        while (remainingDuration > 0)
        {
            remainingDuration -= 1f;
            yield return new WaitForSeconds(1f);
        }

        playerController.SetMoveSpeed(cachedSpeed);
        currentEffect = EffectType.None;
    }

    IEnumerator Poison(float duration)
    {
        //PARTICLE EFFECT
        SpawnParticleEffect(effects[0]);

        float damage = 5f;
        remainingDuration = duration;
        while (remainingDuration > 0)
        {
            playerAttributes.DecreaseHealth(damage);
            remainingDuration -= 1f;
            yield return new WaitForSeconds(1f);
        }
        currentEffect = EffectType.None;
    }

    void SpawnParticleEffect(UnityEngine.GameObject vfx)
    {
        if(vfx == null) { Debug.Log("VFX is null"); return;}
        vfxController.InstantiateVFXAboveHead(vfx);
    }
}
