using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] Transform vfxHitPosition;
    GameObject newVFX;


    public void InstantiateVFXAtHitPosition(GameObject vfx)
    {
        newVFX = Instantiate(vfx, vfxHitPosition.position, Quaternion.identity);
        StartCoroutine(DestroyVFX());
    }

    public IEnumerator DestroyVFX()
    {
        yield return new WaitForSeconds(2f);
        Destroy(newVFX);
    }
}
