using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Interface_InGame : MonoBehaviour
{
    public CanvasGroup groupHUD;
    public CanvasGroup groupLevelStart;
    public CanvasGroup groupLevelComplete;

    public Text hudTime;
    public Text hudScore;
    public Slider hudMagic;

    float m_magicAmount;

    void Awake()
    {
        groupLevelStart.gameObject.SetActive(false);
        groupLevelComplete.gameObject.SetActive(false);
    }

    void Start ()
    {
        SystemsManager.m_Score.Init();
    }

    IEnumerator StartingNewLevel()
    {
        groupLevelStart.gameObject.SetActive(true);
        groupLevelStart.alpha = 1f;
        //animate text moving onto screen
        //count down 
        //send signal to start game
        yield return null;
    }
	
	private void LateUpdate()
	{
		if ((SystemsManager.m_Game.getState () == Game.GameState.Testing || SystemsManager.m_Game.getState () == Game.GameState.Gameplay) &&
            SystemsManager.m_Player != null)
        {
			hudMagic.value = SystemsManager.m_Player.m_magicCurrent / SystemsManager.m_Player.m_magicMax;
            
			hudTime.text = SystemsManager.m_Timer.GetTime().ToString("00.0");

            hudScore.text = SystemsManager.m_Score.m_levelScore.ToString("000");
        }
	}

    public IEnumerator LevelComplete()
    {
        groupLevelComplete.gameObject.SetActive(true);
        groupLevelComplete.alpha = 1f;
        //animate congrats & score text moving up
        //animate score adding up
        StartCoroutine("DecreaseHUDTexts");

        StartCoroutine("IncreaseGameOverTexts");
        //put game in state where it will return to main or restart level
        yield return null;
    }

    public IEnumerator DecreaseHUDTexts()
    {
        int score = (int)SystemsManager.m_Score.m_levelScore;
        int time = (int)SystemsManager.m_Timer.GetTime();

        int highest = 0;
        if (score > time)
            highest = score;
        else
            highest = time;
        for (int i = 0; i < highest; i++)
        {
            if(score > 0)
                score -= 1;

            hudScore.text = time.ToString("000");

            if (time > 0)
                time -= 1;
            
            hudTime.text = score.ToString("00.0");

            yield return new WaitForEndOfFrame();
        }
    }
}
