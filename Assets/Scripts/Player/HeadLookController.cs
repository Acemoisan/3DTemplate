using UnityEngine;

public class HeadLookController : MonoBehaviour
{
    public Transform headTransform; // Assign the player's head transform
    public Camera playerCamera;     // Assign the main camera

    //TODO This only works when animator is off!!!
    // void Update()
    // {
    //     // Get the forward direction of the camera
    //     Vector3 cameraForward = playerCamera.transform.forward;

    //     // Set the head's forward direction to match the camera's
    //     headTransform.forward = new Vector3(cameraForward.x, headTransform.forward.y, cameraForward.z);
    // }
}
