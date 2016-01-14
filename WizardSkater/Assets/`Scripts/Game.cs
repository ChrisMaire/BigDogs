using System.Collections;
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
        Testing,
        Create
    };
    private GameState m_State;

    public LevelNumberMessenger m_levelNum;

    public bool m_startedTheGame;
    public bool m_finishedTheGame;
    public bool m_paused;
    private bool m_inputFreeze;

    public Level m_currentLevel;

    private void Awake()
    {
        m_levelNum = FindObjectOfType<LevelNumberMessenger>();

        m_startedTheGame = false;

        m_paused = false;

        Time.timeScale = 1;
    }

    public void Start()
    {
        if (SystemsManager.m_interCreate == null)
            setState(GameState.LevelComplete);
        else
            setState(GameState.Create);
    }

    public void InitGame()
    {
        m_currentLevel = SystemsManager.m_Level;

        if (m_levelNum)
        {
            m_currentLevel.m_levelOrder = m_levelNum.m_level;
            Debug.Log("starting up level number " + m_levelNum.m_level);

            switch (m_levelNum.m_level)
            {
                case 1:
                {
                    SystemsManager.m_interGame.ChangePalette(0);
                    m_currentLevel.m_fileName = "Level1.xml";
                }
                    break;
                case 2:
                {
                    SystemsManager.m_interGame.ChangePalette(1);
                    m_currentLevel.m_fileName = "Level2.xml";
                }
                    break;
                case 3:
                {
                    SystemsManager.m_interGame.ChangePalette(2);
                    m_currentLevel.m_fileName = "Level3.xml";
                }
                    break;
                case 4:
                {
                    int rando = Random.Range(0, 2);
                    SystemsManager.m_interGame.ChangePalette(rando);
                    m_currentLevel.m_fileName = "Level4.xml";
                }
                    break;
                default:
                {
                    m_currentLevel.m_fileName = "Level1.xml";
                }
                    break;
            }
        }

        m_currentLevel.InitLevel();

        setState(GameState.LevelIntro);

        SpawnPlayer();

        SystemsManager.m_interGame.StartCoroutine("LevelIntro");
    }

    public void StartGame()
    {
        setState(GameState.Gameplay);
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

    public void Pause()
    {
        setState(GameState.Pause);
        Time.timeScale = 0F;
        SystemsManager.m_interGame.Pause();
    }

    public void UnPause()
    {
        setState(GameState.Gameplay);
        Time.timeScale = 1F;
        SystemsManager.m_interGame.UnPause();
    }

    public bool CanPlay()
    {
        return (getState() == GameState.Gameplay || getState() == GameState.Testing);
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
