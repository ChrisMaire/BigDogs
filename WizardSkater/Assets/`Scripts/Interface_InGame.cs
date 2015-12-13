using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Interface_InGame : MonoBehaviour {

	public Text Timer;
	public Text Score;
	public Slider Speed;
	public Slider Action;

	float maxspeed;
	float speed;
	float valuehold;

	void Start ()
    {
        SystemsManager.m_Score.Init();
    }
	
	private void LateUpdate()
	{
		if ((SystemsManager.m_Game.getState () == Game.GameState.Testing ||
            SystemsManager.m_Game.getState () == Game.GameState.Gameplay) &&
            SystemsManager.m_Player != null)
        {
			speed = SystemsManager.m_Player.m_moveSpeed;
			maxspeed = SystemsManager.m_Player.m_maxSpeedTotal;

			Speed.value = speed / maxspeed;

			Timer.text = Time.time.ToString();

			Score.text = "00000";
		}
	}
			/*	if(!m_player.GetComponent<Player>().m_moveChangeReady){
								ActionBar();
			}
		} */
	

	/*
	private void ActionBar()
	{
	
		if (active) { valuehold = 1;
			Action.value = 1;
		}
		else {
			Action.value = valuehold - .1f;
		}

	}*/ 
}
