using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Interface_InGame : MonoBehaviour {

	public Text Timer;
	public Text Score;
	public Slider Speed;
	public Slider Action;
	private GameObject m_player;
	private bool active;

	float maxspeed;
	float speed;
	float valuehold;

	// Use this for initialization
	void Start () {

		bool active = true;
	   
	}
	
	private void LateUpdate()
	{
		if (m_player == null) {
			m_player = SystemsManager.m_Player.gameObject;
			if (m_player == null) {
				Debug.Log ("where is pretty baby???");
			}
		}
		
		if ((SystemsManager.m_Game.getState () == Game.GameState.Testing || SystemsManager.m_Game.getState () == Game.GameState.Gameplay) &&
			m_player != null) {
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
