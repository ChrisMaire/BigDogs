using UnityEngine;
using System.Collections;

public class LevelNumberMessenger : MonoBehaviour
{
    public int m_level = -1;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}