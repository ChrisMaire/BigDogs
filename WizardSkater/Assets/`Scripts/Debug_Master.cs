using UnityEngine;
using System.Collections;

public class Debug_Master : MonoBehaviour
{
    public bool debug = false;
    public static bool m_debug = false;

    void Awake()
    {
        UpdateInternal();
    }

    void Update()
    {
        UpdateInternal();
    }

    void UpdateInternal()
    {
        if (m_debug == false && debug == true)
            setStatus(true);
        else if (m_debug == true && debug == false)
            setStatus(false);
    }

    public static bool getStatus()
    {
        return m_debug;
    }
    public void setStatus(bool status)
    {
        m_debug = status;
    }
}
