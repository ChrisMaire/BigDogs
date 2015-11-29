using UnityEngine;
using System.Collections;

public class DebugReloadScene : MonoBehaviour
{
    private void Update()
    {
        if (Debug_Master.m_debug)
        {
            if (Input.GetKeyUp("l"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
