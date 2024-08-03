using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody itemRigidbody = transform.GetComponent<Rigidbody>();

        // Check if the item has a Rigidbody component
        if (itemRigidbody != null)
        {
            // Define the force direction and magnitude
            Vector3 forceDirection = (Vector3.up + Random.insideUnitSphere).normalized;
            float forceMagnitude = 5.0f; // Adjust the magnitude as needed

            // Apply the force to the item
            itemRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Item does not have a Rigidbody component.");
        }
    }


}
