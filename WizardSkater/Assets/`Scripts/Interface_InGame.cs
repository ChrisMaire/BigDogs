using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Interface_InGame : MonoBehaviour
{
    public Animator m_animator;

    public CanvasGroup groupHUD;
    public CanvasGroup groupLevelStart;
    public CanvasGroup groupLevelComplete;

    public Text hudTime;
    public Text hudScore;
    public Slider hudMagic;

    public Text completeTime;
    public Text completeScore;

    public Renderer stageBG;
    public Renderer stageSet;
    public Renderer stageFG;

    public List<Texture> stageBGTypes;
    public List<Texture> stageSetTypes;
    public List<Texture> stageFGTypes;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        groupLevelStart.gameObject.SetActive(false);
        groupLevelComplete.gameObject.SetActive(false);
    }

    void Start ()
    {
        SystemsManager.m_Score.Init();
    }

    public void ChangePalette(int type)
    {
        stageBG.material.mainTexture = stageBGTypes[type];
        stageSet.material.mainTexture = stageSetTypes[type];
        stageFG.material.mainTexture = stageFGTypes[type];
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

    public IEnumerator LevelIntro()
    {
        groupLevelStart.gameObject.SetActive(true);
        groupLevelStart.alpha = 1f;

        m_animator.SetTrigger("LevelIntro");
        //animate text coming down

        yield return  new WaitForSeconds(3f);
        
        SystemsManager.m_Game.StartGame();

        groupLevelStart.alpha = 0f;
        groupLevelStart.gameObject.SetActive(false);
    }

    public IEnumerator LevelComplete()
    {
        groupLevelComplete.gameObject.SetActive(true);

        groupLevelComplete.alpha = 1f;

        m_animator.SetTrigger("LevelComplete");
        yield return null;

        //animate congrats & score text moving up
        //animate score adding up
        StartCoroutine("DecreaseHUDTexts");

        StartCoroutine("IncreaseGameOverTexts");
        yield return null;

        //put game in state where it will return to main or restart level
        SystemsManager.m_Game.setState(Game.GameState.PostOutro);
    }

    public IEnumerator DecreaseHUDTexts()
    {
        float score = SystemsManager.m_Score.m_levelScore;
        float time = SystemsManager.m_Timer.GetTime();

        float highest = 0f;
        if (score > time)
            highest = score;
        else
            highest = time;

        for (int i = 0; i < highest; i++)
        {
            if (score > 0)
                score -= 3;
            else
                score = 0;

            hudScore.text = score.ToString("000");

            if (time > 0)
                time -= Time.fixedDeltaTime*5f;
            else
                time= 0;

            hudTime.text = time.ToString("00.0");

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator IncreaseGameOverTexts()
    {
        float score = SystemsManager.m_Score.m_levelScore;
        float time = SystemsManager.m_Timer.GetTime();

        float highest = 0f;
        if (score > time)
            highest = score;
        else
            highest = time;

        float scoreCurrent = 0f;
        float timeCurrent = 0f;
for (int i = 0; i < highest; i++)
        {
            if (scoreCurrent < score)
                scoreCurrent += 3;
            else
                scoreCurrent = score;

            completeScore.text = scoreCurrent.ToString("000");

            if (timeCurrent < time)
                timeCurrent += Time.fixedDeltaTime*5f;
            else
                timeCurrent = time;

            completeTime.text = timeCurrent.ToString("00.0");

            yield return new WaitForEndOfFrame();
        }
    }
}
