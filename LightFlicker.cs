using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light torchLight;       // The light component on your torch prefab
    public float minIntensity = 50f;  // Minimum light intensity
    public float maxIntensity = 100f;  // Maximum light intensity
    public float flickerSpeed = 0.1f;  // Speed of the flicker

    private float targetIntensity;     // Target intensity to lerp towards
    private float timePassed = 0f;     // Timer to control the lerp speed

    void Start()
    {
        // Set initial target intensity
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        timePassed += Time.deltaTime * flickerSpeed;

        // Lerp the intensity of the light towards the target intensity
        torchLight.intensity = Mathf.Lerp(torchLight.intensity, targetIntensity, timePassed);

        // Once the light intensity gets close enough to the target, choose a new random target intensity
        if (Mathf.Abs(torchLight.intensity - targetIntensity) < 0.05f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timePassed = 0f;  // Reset the timer to start a new lerp
        }
    }
}
