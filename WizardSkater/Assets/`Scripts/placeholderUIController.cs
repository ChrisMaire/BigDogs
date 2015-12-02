using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class placeholderUIController : MonoBehaviour {

	public Text time;
	public Text Score;
	public Slider Speed;
	public Component Player;
	GameObject Wizard;

	float maxspeed;
	float speed;

	// Use this for initialization
	void Start () {

	 
	   
	}
	
	// Update is called once per frame
	void Update () {
	
		//is wizard?
		if (GameObject.Find ("Wizard"))
		{	
			Wizard = GameObject.Find ("Wizard");

			Player = Wizard.GetComponent<Player> ();

			//handles speed meter
			 

		}
	}
}
