using UnityEngine;
using System.Collections;

public class SystemsManager : MonoBehaviour
{
    public static Game m_Game;
    public Game Game;
    public static Prefabs m_Prefabs;
    public Prefabs Prefabs;
    public static Sprites m_Sprites;
    public Sprites Sprites;
    public static InputHandler m_Input;
    public InputHandler InputHandler;
    public static Timer m_Timer;
    public Timer Timer;
    public static Score m_Score;
    public Score Score;
    public static Particles m_Particles;
    public Particles Particles;
    public static Colors m_Colors;
    public Colors Colors;
    public static TextStrings m_Strings;
    public TextStrings TextStrings;
    public static SoundFX m_SoundFX;
    public SoundFX SoundFX;
    public static Music m_Music;
    public Music Music;
    public static Camera_Wizard m_Camera;
    public Camera_Wizard Camera;

    private Player Player;
    public static Player m_Player;
    private Level Level;
    public static Level m_Level;

    void Awake()
    {
        m_Colors = Colors;
        m_Game = Game;
        m_Music = Music;
        m_Particles = Particles;
        m_Prefabs = Prefabs;
        m_Sprites = Sprites;
        m_Score = Score;
        m_SoundFX = SoundFX;
        m_Timer = Timer;
        m_Camera = Camera;
        m_Strings = TextStrings;
        m_Input = InputHandler;
    }

    void Start()
    {
        //Particles.InitObjectPools();
        Level = FindObjectOfType<Level>();
        m_Level = Level;

        Game.InitGame();

        m_Player = FindObjectOfType<Player>();
        Player = m_Player;
    }

    void Update()
    {
        m_SoundFX.transform.position = m_Camera.transform.position;
        m_Music.transform.position = m_Camera.transform.position;
    }
}
