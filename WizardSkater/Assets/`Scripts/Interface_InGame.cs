using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Interface_InGame : MonoBehaviour
{
    public Animator m_animator;
    public EventSystem m_events;

    public CanvasGroup groupHUD;
    public CanvasGroup groupLevelStart;
    public CanvasGroup groupLevelComplete;
    public CanvasGroup groupPause;

    public Text hudTime;
    public Text hudScore;
    public Slider hudMagic;

    public Image hudScreenFader;
    public float m_fadeTime = 0.25f;

    public Text completeTime;
    public Text completeScore;

    public Renderer stageBG;
    public Renderer stageSet;
    public Renderer stageFG;

    public List<Texture> stageBGTypes;
    public List<Texture> stageSetTypes;
    public List<Texture> stageFGTypes;

    public float m_animIntroTime = 3.35f;

    public int m_pauseSelector;
    public Button[] m_pauseSelectButtons;
    public bool m_pauseInputReady = false;
    public float m_pauseInputTimer;
    public float m_pauseInputDelayTime = 0.1f;

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        m_animator = GetComponent<Animator>();

        if(groupLevelStart)
            groupLevelStart.gameObject.SetActive(false);
        if(groupLevelComplete)
            groupLevelComplete.gameObject.SetActive(false);
        if (groupPause)
        {
            groupPause.alpha = 0f;
            groupPause.gameObject.SetActive(false);
        }
    }

    void Start ()
    {
        if(SystemsManager.m_Score)
            SystemsManager.m_Score.Init();

        StartCoroutine("FadeInScene");
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
        else if (SystemsManager.m_Game.getState() == Game.GameState.PostOutro)
        {
            if (SystemsManager.m_Input.inp_Skate)
            {
                StartCoroutine("FadeOutScene", true);
            }
            if (SystemsManager.m_Input.inp_Turbo)
            {
                StartCoroutine("FadeOutScene", false);
            }
        }

	    if (SystemsManager.m_Game.getState() == Game.GameState.Pause)
	    {
	        if (m_pauseInputReady == false)
	        {

	            if (m_pauseInputTimer < m_pauseInputDelayTime)
	            {
	                m_pauseInputTimer += Time.fixedDeltaTime;
                }
	            else
	            {
	                if (SystemsManager.m_Input.IsInputClear())
	                {
	                    m_pauseInputReady = true;
                        m_pauseInputTimer = 0;
                    }
	            }
	        }
	        else
	        {
	            bool input = false;
                //if (SystemsManager.m_Input.inp_D_Up)
                //{

                //       m_pauseInputReady = false;
                //       input = true;
                //   }
                //else if (SystemsManager.m_Input.inp_D_Down)
                //{

                //       m_pauseInputReady = false;
                //       input = true;
                //   }
                //else 
                if (SystemsManager.m_Input.inp_D_Left)
	            {
                    m_pauseSelector--;
                    m_pauseInputReady = false;
                    input = true;
                }
	            else if (SystemsManager.m_Input.inp_D_Right)
	            {
                    m_pauseSelector++;
                    m_pauseInputReady = false;
                    input = true;
                }
                else if (SystemsManager.m_Input.inp_Skate)
                {
                    switch (m_pauseSelector)
                    {
                        case 0:
                            {
                                SystemsManager.m_Game.UnPause();
                            }
                            break;
                        case 1:
                            {
                                StartCoroutine("FadeOutScene", false);
                                SystemsManager.m_Game.UnPause();
                            }
                            break;
                        case 2:
                            {
                                StartCoroutine("FadeOutScene", true);
                                SystemsManager.m_Game.UnPause();
                            }
                            break;
                        case 3:
                        {
                            if (Application.isEditor == false)
                                    StartCoroutine("Quit");
                                SystemsManager.m_Game.UnPause();
                        }
                        break;
                        default:
                            {
                                SystemsManager.m_Game.UnPause();
                            }
                            break;
                    }
                }

	            if (input)
	            {
	                if (m_pauseSelector < 0)
	                    m_pauseSelector = m_pauseSelectButtons.Length - 1;
                    else if (m_pauseSelector >= m_pauseSelectButtons.Length)
                        m_pauseSelector = 0;

                    m_pauseSelectButtons[m_pauseSelector].Select();
                    m_events.firstSelectedGameObject = m_pauseSelectButtons[m_pauseSelector].gameObject;
                }
	        }
	    }
	}

    public void Pause()
    {
        Music.source.pitch = 0.75f;
        Color screenFade = Color.black;
        screenFade.a = 0.25f;
        hudScreenFader.color = screenFade;
        groupPause.gameObject.SetActive(true);
        m_pauseSelector = 0;
        m_pauseSelectButtons[m_pauseSelector].Select();
        m_events.firstSelectedGameObject = m_pauseSelectButtons[m_pauseSelector].gameObject;
        groupPause.alpha = 1f;
        m_pauseInputReady = true;
    }

    public void UnPause()
    {
        Music.source.pitch = 1f;
        Color screenFade = Color.black;
        screenFade.a = 0;
        hudScreenFader.color = screenFade;
        groupPause.gameObject.SetActive(false);
        groupPause.alpha = 0f;
    }

    private IEnumerator Quit()
    {
        SystemsManager.m_SoundFX.uiOneShot_Submit();
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }


    public IEnumerator FadeInScene()
    {
        Color screenFade = hudScreenFader.color;
        for (float time = 0f; time < m_fadeTime; time += Time.deltaTime)
        {
            //Debug.Log("fade step " + time + " vs " + m_fadeTime);
            screenFade.a -= 0.05f;
            hudScreenFader.color = screenFade;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        screenFade.a = 0f;
        hudScreenFader.color = screenFade;
    }

    public IEnumerator FadeOutScene(bool returningToMenu)
    {
        Color screenFade = hudScreenFader.color;
        for (float time = 0f; time < m_fadeTime; time += Time.deltaTime)
        {
            screenFade.a += 0.05f;
            hudScreenFader.color = screenFade;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        screenFade.a = 1f;
        hudScreenFader.color = screenFade;

        if(returningToMenu)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(1);
    }

    public IEnumerator LevelIntro()
    {
        yield return new WaitForSeconds(1f);

        groupLevelStart.gameObject.SetActive(true);
        groupLevelStart.alpha = 1f;
        m_animator.SetTrigger("LevelIntro");

        yield return  new WaitForSeconds(m_animIntroTime);
        
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
        yield return new WaitForSeconds(2f);

        //put game in state where it will return to main or restart level
        SystemsManager.m_Game.setState(Game.GameState.PostOutro);
    }

    public IEnumerator DecreaseHUDTexts()
    {
        float score = int.Parse(hudScore.text);
        float time = SystemsManager.m_Timer.GetTime() / (Time.fixedDeltaTime*10f);

        float highest = 0f;
        if (score > time)
            highest = score;
        else
            highest = time;

        time = SystemsManager.m_Timer.GetTime();

        for (int i = 0; i < highest; i++)
        {
            if (score > 0)
                score -= 10;
            else
                score = 0;

            hudScore.text = score.ToString("000");

            if (time > 0)
                time -= Time.fixedDeltaTime*10f;
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
                scoreCurrent += 5;
            else
                scoreCurrent = score;

            completeScore.text = scoreCurrent.ToString("000");

            if (timeCurrent < time)
                timeCurrent += Time.fixedDeltaTime*10f;
            else
                timeCurrent = time;

            completeTime.text = timeCurrent.ToString("00.0");

            yield return new WaitForEndOfFrame();
        }
    }
}
