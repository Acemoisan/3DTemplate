using System.Collections;
using UnityEngine;
using TMPro;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    public float updateInterval = 0.1f; // Interval in seconds

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(FaceCameraCoroutine());
    }

    IEnumerator FaceCameraCoroutine()
    {
        while (true)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
