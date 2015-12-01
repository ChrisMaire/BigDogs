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

    public void InitGame()
    {
        m_startedTheGame = false;

        m_paused = false;

        Time.timeScale = 1;

        m_State = GameState.Testing;

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.Find("Player");
        if(player == null)
            player = Instantiate(SystemsManager.m_Prefabs.m_player);
        else
            Debug.Log("player already in scene!!");

        player.name = "Wizard!";
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
