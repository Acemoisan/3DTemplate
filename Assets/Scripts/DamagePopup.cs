using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DamagePopup : MonoBehaviour
{
    //create a popup
    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        Vector3 randomPosition = position + Random.insideUnitSphere * 1;
        Transform damagePopupTransform = Instantiate(GameAssets.instance.damagePopup, randomPosition, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);
        return damagePopup;
    }


    TextMeshPro textMesh;



    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        if(textMesh == null)
        {
            Debug.LogError("No TextMeshPro component found on " + gameObject.name);
        }
    }


    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        float moveYSpeed = 7f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
    }
}
