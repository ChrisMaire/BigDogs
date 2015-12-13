using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Interface_InGame : MonoBehaviour {

	public Text Time;
	public Text Score;
	public Slider Magic;

	float m_magicAmount;

    private string m_emptyNum = "00.0";

    void Start ()
    {
        SystemsManager.m_Score.Init();
    }
	
	private void LateUpdate()
	{
		if ((SystemsManager.m_Game.getState () == Game.GameState.Testing || SystemsManager.m_Game.getState () == Game.GameState.Gameplay) &&
            SystemsManager.m_Player != null)
        {
			Magic.value = SystemsManager.m_Player.m_magicCurrent / SystemsManager.m_Player.m_magicMax;
            
			Time.text = SystemsManager.m_Timer.GetTime().ToString(m_emptyNum);
            Debug.Log("time is " + SystemsManager.m_Timer.GetTime().ToString());
			Score.text = SystemsManager.m_Score.m_levelScore.ToString(m_emptyNum);
        }
	}
}
