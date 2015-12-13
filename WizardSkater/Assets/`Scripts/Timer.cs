using UnityEngine;

public class Timer : MonoBehaviour
{
    private float m_timeCounter;
    public bool m_timePause;

    void Update()
    {
        if (SystemsManager.m_Game.getState() == Game.GameState.Gameplay ||
            SystemsManager.m_Game.getState() == Game.GameState.Testing)
        {
            if (m_timePause == false)
            {
                m_timeCounter += Time.deltaTime;
            }
        }
    }

    public void SetTimePause(bool toggle)
    {
        m_timePause = toggle;
    }

    public void ResetTime()
    {
        m_timeCounter = 0;
    }

    public float GetTime()
    {
        return m_timeCounter;
    }
}