﻿using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        LevelIntro,
        Gameplay,
        Pause,
        LevelComplete,
        PostOutro,
        Testing
    };
    private GameState m_State;

    public bool m_startedTheGame;
    public bool m_finishedTheGame;
    public bool m_paused;
    private bool m_inputFreeze;

    public Level m_currentLevel;

    private void Awake()
    {
        m_startedTheGame = false;

        m_paused = false;

        Time.timeScale = 1;

        m_State = GameState.LevelComplete;
    }

    public void InitGame()
    {
        m_currentLevel = SystemsManager.m_Level;
        m_currentLevel.InitLevel();

        m_State = GameState.LevelIntro;

        SpawnPlayer();

        SystemsManager.m_interGame.StartCoroutine("LevelIntro");
    }

    public void StartGame()
    {
        m_State = GameState.Gameplay;
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            player = Instantiate(SystemsManager.m_Prefabs.m_player);
            //Debug.Log("player created");
        }
        else
        {
            Destroy(player);
            player = Instantiate(SystemsManager.m_Prefabs.m_player);
            Debug.Log("player already in scene!!");
        }
        
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
