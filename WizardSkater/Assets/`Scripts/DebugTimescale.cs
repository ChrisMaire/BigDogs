using UnityEngine;

public class DebugTimescale : MonoBehaviour
{
    private void Update()
    {
        if (Debug_Master.m_debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 0.05F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 0.075F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 0.1F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 0.15F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Time.timeScale = 0.2F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Time.timeScale = 0.35F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Time.timeScale = 0.5F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Time.timeScale = 0.65F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Time.timeScale = 0.8F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Time.timeScale = 1F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
        }
    }
}