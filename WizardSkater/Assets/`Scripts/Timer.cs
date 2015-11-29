using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timeCounter;
    public bool timePause;

    public void UpdateTime()
    {
        if (SystemsManager.m_Game.getState() == Game.GameState.Gameplay)
        {
            if (timePause)
                timePause = false;
        }
        else
        {
            if (timePause == false)
                timePause = true;
        }

        if (timePause == false)
        {
            timeCounter += Time.deltaTime;
            //SystemsManager.m_Score.ChangeLevelTime(timeCounter);
            //SystemsManager.m_Interface.ChangeTimeOnHUD(timeCounter.ToString("##.##"));
        }
    }

    public void ResetTime()
    {
        timeCounter = 0;

        //SystemsManager.m_Interface.ChangeTimeOnHUD(timeCounter.ToString("##.##"));
    }

    public float GetTime()
    {
        return timeCounter;
    }

    public float GetTotalTime()
    {
        return timeCounter;
    }
}