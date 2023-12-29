using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] Transform vfxHitPosition;
    [SerializeField] Transform aboveHeadVFXPosition;
    GameObject newVFX;


    public void InstantiateVFXAtHitPosition(GameObject vfx)
    {
        newVFX = Instantiate(vfx, vfxHitPosition.position, Quaternion.identity);
        StartCoroutine(DestroyVFX());
    }

    public void InstantiateVFXAboveHead(GameObject vfx)
    {
        newVFX = Instantiate(vfx, aboveHeadVFXPosition.position, Quaternion.identity);
        StartCoroutine(DestroyVFX());
    }

    public IEnumerator DestroyVFX()
    {
        yield return new WaitForSeconds(5f);
        Destroy(newVFX);
    }
}
