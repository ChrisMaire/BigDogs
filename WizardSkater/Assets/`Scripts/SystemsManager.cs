using UnityEngine;
using System.Collections;

public class SystemsManager : MonoBehaviour
{
    public static Game m_Game;
    public Game Game;
    public static Prefabs m_Prefabs;
    public Prefabs Prefabs;
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


    void Awake()
    {
        m_Colors = Colors;
        m_Game = Game;
        m_Music = Music;
        m_Particles = Particles;
        m_Prefabs = Prefabs;
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

        m_Player = FindObjectOfType<Player>();
        Player = m_Player;
        Player.name = "Wizard!";
    }

    void Update()
    {
        transform.position = m_Camera.transform.position;
    }
}
