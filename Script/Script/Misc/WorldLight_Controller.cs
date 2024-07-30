using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight_Controller : MonoBehaviour
{
    public Light2D worldLight; // Assign the 2D light component in the Inspector
    public float minIntensity = 0.04f; // Minimum light intensity
    public float maxIntensity = 1f; // Maximum light intensity

    private ClockScript clockScript; // Reference to the ClockScript
    private bool isIncreasing = true; // Flag to track whether light intensity is increasing or decreasing

    public GameObject[] objectsToToggle; // Array to store GameObjects to toggle

    void Start()
    {
        clockScript = GameObject.FindObjectOfType<ClockScript>();
    }

    void Update()
    {
        if (clockScript.currentTime >= 360f && isIncreasing)
        {
            float timeNormalized = (clockScript.currentTime - 360f) / 360f; // Normalize time between 0 and 1
            float lightIntensity = Mathf.Lerp(minIntensity, maxIntensity, timeNormalized * 3); // Interpolate light intensity
            worldLight.intensity = lightIntensity;
            if (clockScript.currentTime >= 1050f)
            {
                isIncreasing = false;
            }
        }
        else if (!isIncreasing)
        {
            float timeNormalized = (clockScript.currentTime - 1050f) / 360f; // Normalize time between 0 and 1
            float lightIntensity = Mathf.Lerp(maxIntensity, minIntensity, timeNormalized * 3); // Interpolate light intensity
            worldLight.intensity = lightIntensity;
            if (clockScript.currentTime < 360f)
            {
                isIncreasing = true;
            }
        }
        else
        {
            worldLight.intensity = minIntensity; // Set light intensity to minimum when time is less than 360
        }


        if (clockScript.currentTime >= 960f && clockScript.currentTime <= 1180f)
        {
            float orangeLerp = (clockScript.currentTime - 960f) / 60f; // Normalize time between 0 and 1
            Color orangeColor = Color.Lerp(Color.white, new Color(1f, 0.58f, 0f), orangeLerp); // Interpolate color
            worldLight.color = orangeColor;
            ToggleObjects(true);
        }
        else if (clockScript.currentTime >= 1180f && clockScript.currentTime <= 1250f)
        {
            float whiteLerp = (clockScript.currentTime - 1180f) / 60f; // Normalize time between 0 and 1
            Color whiteColor = Color.Lerp(new Color(1f, 0.58f, 0f), Color.white, whiteLerp); // Interpolate color
            worldLight.color = whiteColor;
        }

        else if (clockScript.currentTime >= 300f && clockScript.currentTime <= 400f)
        {
            float blueLerp = (clockScript.currentTime - 300f) / 60f; // Normalize time between 0 and 1
            Color blueColor = Color.Lerp(Color.white, new Color(0f, 0.38f, 1f), blueLerp); // Interpolate color
            worldLight.color = blueColor;
        }
        else if (clockScript.currentTime >= 400f && clockScript.currentTime <= 450f)
        {
            float whiteLerp = (clockScript.currentTime - 400f) / 60f; // Normalize time between 0 and 1
            Color whiteColor = Color.Lerp(new Color(0f, 0.38f, 1f), Color.white, whiteLerp); // Interpolate color
            worldLight.color = whiteColor;
            ToggleObjects(false);
        }
        else
        {
            worldLight.color = Color.white; // Reset color to white when not in the orange range
        }
    }

    public void ToggleObjects(bool isActive)
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(isActive);
        }
    }
}