using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Interlude,
        Gameplay,
        Pause,
        Testing
    };
    private GameState m_State;

    public bool m_startedTheGame;
    public bool m_finishedTheGame;
    public bool m_paused;
    private bool m_inputFreeze;

    private void Awake()
    {
        m_startedTheGame = false;

        m_paused = false;

        Time.timeScale = 1;
    }

    public GameState getState()
    {
        return m_State;
    }

    public void setState(GameState state)
    {
        m_State = state;
    }
}
