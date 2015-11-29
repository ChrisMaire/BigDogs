using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Camera cam;
    private Vector3 cameraPos;
    private float shake;
    public float ShakeAmount = 0.25f;
    public float DecreaseFactor = 1.0f;

    private void Awake()
    {
        // Go and find the camera.
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            // Print an error.
            Debug.Log("CameraShake: Unable to find 'Camera_Wizard' component attached to GameObject.");
        }
    }

    private void Update()
    {
        // Check if the screen should be shaking.
        if (shake > 0.0f)
        {
            // Shake the camera.
            cam.transform.localPosition = cam.transform.localPosition +
                                             (Random.insideUnitSphere * ShakeAmount * shake);

            // Reduce the amount of shaking for next tick.
            shake -= Time.deltaTime * DecreaseFactor;

            // Check to see if we've stopped shaking.
            if (shake <= 0.0f)
            {
                // Clamp the shake amount back to zero, and reset the camera position to our cached value.
                shake = 0.0f;
                cam.transform.localPosition = cameraPos;
                //				gameObject.GetComponent<Camera_TopDown>().Shaking = false;
            }
        }
    }

    public void Shake(float amount)
    {
        // Check if we're already shaking.
        if (shake <= 0.0f)
        {
            // If we aren't, cache the camera position.
            cameraPos = cam.transform.position;

            //			gameObject.GetComponent<CameraController>().Shaking = true;
        }

        // Set the 'shake' value.
        shake = amount;
    }
}