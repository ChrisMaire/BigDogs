using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine.EventSystems;

public class Interface_MainMenu : MonoBehaviour
{
    public EventSystem m_events;
    public Animator m_animator;

    public bool m_transitioning;

    public int m_menuSelector;
    public Button[] m_menuSelectButtons;

    public int m_levelSelector;
    public Button[] m_levelSelectButtons;
    public List<Text> m_levelSelectBestTimes;

    public bool m_menuInputReady = false;
    public float m_menuInputTimer;
    public float m_menuInputDelayTime = 0.1f;

    public enum MenuState
    {
        AttractMode,
        MainMenu,
        LevelSelect
    };

    public MenuState m_state;

    public void Update()
    {
        if (m_menuInputReady == false)
        {
            if (m_menuInputTimer < m_menuInputDelayTime)
            {
                m_menuInputTimer += Time.deltaTime;
            }
            else
            {
                m_menuInputReady = true;
                m_menuInputTimer = 0;
            }
        }

        if (m_state == MenuState.MainMenu)
        {
            if (SystemsManager.m_Input.inp_Jump)
            {
                if (getMenuSelector() == 0 && SystemsManager.m_Game.m_startedTheGame == false)
                {
                    StartNewGame();
                }
                else if (getMenuSelector() == 1)
                {
                    GoLevelSelect(false);
                }
                else if (getMenuSelector() == 2)
                {
                    Application.Quit();
                }
                else if (getMenuSelector() == 4)
                {
                    //how me do thing
                }
            }
            else
            {
                if (m_menuInputReady)
                {
                    if (SystemsManager.m_Input.inp_D_Up)
                    {
                        StartCoroutine("ChangeMenuSelector", "up");
                        m_menuInputReady = false;
                    }
                    else if (SystemsManager.m_Input.inp_D_Down)
                    {
                        StartCoroutine("ChangeMenuSelector", "down");
                        m_menuInputReady = false;
                    }
                    else if (SystemsManager.m_Input.inp_D_Left)
                    {
                        StartCoroutine("ChangeMenuSelector", "left");
                        m_menuInputReady = false;
                    }
                    else if (SystemsManager.m_Input.inp_D_Right)
                    {
                        StartCoroutine("ChangeMenuSelector", "right");
                        m_menuInputReady = false;
                    }
                }
            }
        }
        if (m_state == MenuState.MainMenu)
        {
            if (m_menuInputReady)
            {
                if (SystemsManager.m_Input.inp_Jump)
                {

                }
                else
                {
                    //if (SystemsManager.m_Input.inp_Crouch)
                    //{
                    //    GoLevelSelect(true);
                    //    SystemsManager.m_Game.setState(Game.GameState.Title);
                    //    m_menuInputReady = false;
                    //}

                    if (SystemsManager.m_Input.inp_D_Up)
                    {
                        StartCoroutine("ChangeMenuSelector", "up");
                        m_menuInputReady = false;
                    }
                    else if (SystemsManager.m_Input.inp_D_Down)
                    {
                        StartCoroutine("ChangeMenuSelector", "down");
                        m_menuInputReady = false;
                    }
                    else if (SystemsManager.m_Input.inp_D_Left)
                    {
                        StartCoroutine("ChangeMenuSelector", "left");
                        m_menuInputReady = false;
                    }
                    else if (SystemsManager.m_Input.inp_D_Right)
                    {
                        StartCoroutine("ChangeMenuSelector", "right");
                        m_menuInputReady = false;
                    }
                }
            }
        }
    }
    public void StartNewGame()
    {
        StartCoroutine("MenuTransitionMainMenu", true);

        SystemsManager.m_Camera.StartCoroutine("StartIntro");
        SystemsManager.m_Game.StartCoroutine("StartNewGame");
    }

    public void StartNewLevelGame(int level)
    {
        Debug.Log("Loading level " + level);

        //SystemsManager.m_Game.m_gameWorldPosition = level;

        GoLevelSelect(true);

        SystemsManager.m_Camera.StartCoroutine("StartIntro");
        SystemsManager.m_Game.StartCoroutine("StartNewLevelGame", level);
    }

    public IEnumerator MenuTransitionMainMenu(bool reverse)
    {
        m_transitioning = true;

        m_animator.ResetTrigger("FadeOutMainMenu");
        m_animator.ResetTrigger("FadeOutLevelSelect");
        m_animator.ResetTrigger("FadeOutHow");

        if (reverse == false)
        {
            //if (groupLevelSelect.gameObject)
            //{
            //    //Debug.Log("mm in");
            //    ToggleLevelOutro(false);
            //    groupGameOver.gameObject.SetActive(false);

            //    groupTitle.gameObject.SetActive(true);
            //    groupTitle.alpha = 0f;
            //    m_menuSelectButtons[m_menuSelector].Select();

            //    m_animator.SetTrigger("FadeOutLevelSelect");

            //    yield return new WaitForSeconds(0.1f);

            //    m_animator.SetTrigger("FadeInMainMenu");
            //    SystemsManager.m_Score.Init();

            yield return new WaitForSeconds(0.2f);

            //    groupLevelSelect.alpha = 0f;
            //    groupLevelSelect.gameObject.SetActive(false);

            //    groupTitle.alpha = 1f;
            //    SystemsManager.m_Game.setState(Game.GameState.Title);
            //}
        }
        else
        {
            //Debug.Log("mm out");
            //m_animator.SetTrigger("FadeOutMainMenu");

            //yield return new WaitForSeconds(0.2f);

            //m_fader.enabled = true;

            //SystemsManager.m_Game.m_startedTheGame = true;

            //SystemsManager.m_Camera.InitAngles();

            ////SystemsManager.m_ScreenWipe.DoFade();
            //m_camMenu.enabled = false;

            yield return new WaitForSeconds(0.2f);

            //groupTitle.gameObject.SetActive(false);
        }

        m_transitioning = false;
    }

    public void GoMainMenu(bool reverse = false)
    {
        if (m_transitioning == false)
            StartCoroutine("MenuTransitionMainMenu", reverse);
    }

    public IEnumerator MenuTransitionLevelSelect(bool reverse)
    {
        m_transitioning = true;

        m_animator.ResetTrigger("FadeOutMainMenu");
        m_animator.ResetTrigger("FadeOutLevelSelect");
        m_animator.ResetTrigger("FadeOutHow");

        if (reverse == false)
        {
            //Debug.Log("ls in");
            //groupLevelSelect.gameObject.SetActive(true);
            //groupLevelSelect.alpha = 0f;
            //m_levelSelectButtons[0].Select();

            //for (int i = 0; i < m_levelSelectButtons.Length; i++) //buttons
            //{
            //    m_levelSelectButtons[i].enabled = true;
            //}

            //for (int i = 0; i < m_levelSelectBestTimes.Count - 1; i += 1) //best times
            //{
            //    float top = SystemsManager.m_Score.BestLevelTimes[i];
            //    string topS = top.ToString("0.00");
            //    if (SystemsManager.m_Score.BestLevelTimes.Count >= i && top != -1)
            //    {
            //        m_levelSelectBestTimes[i].text = topS;
            //        m_levelSelectBestTimesShadows[i].text = topS;
            //    }
            //}
            //for (int i = 1; i < m_levelSelectBestPerfectTimes.Count - 1; i += 1) //best perfect time
            //{
            //    float top = SystemsManager.m_Score.BestLevelPerfectTimes[i];
            //    string topS = top.ToString("0.00");
            //    if (SystemsManager.m_Score.BestLevelPerfectTimes.Count >= i && top != -1)
            //    {
            //        m_levelSelectBestPerfectTimes[i].text = topS;
            //        m_levelSelectBestPerfectTimesShadows[i].text = topS;
            //    }
            //}

            //m_animator.SetTrigger("FadeOutMainMenu");

            //yield return new WaitForSeconds(0.1f);

            //m_animator.SetTrigger("FadeInLevelSelect");

            yield return new WaitForSeconds(0.2f);

            //groupTitle.alpha = 0f;
            //groupTitle.gameObject.SetActive(false);

            //groupLevelSelect.alpha = 1f;
            //SystemsManager.m_Game.setState(Game.GameState.LevelSelect);
        }
        else
        {
            //Debug.Log("ls out");
            //m_animator.SetTrigger("FadeOutLevelSelect");

            //yield return new WaitForSeconds(0.2f);

            //groupLevelSelect.alpha = 0f;

            //if (SystemsManager.m_Game.m_gameWorldPosition == -1)
            //{
            //    m_camMenu.enabled = true;
            //    StartCoroutine("MenuTransitionMainMenu", false);
            //}
            //else
            //{
            //    m_fader.enabled = true;

            //    SystemsManager.m_Game.m_startedTheGame = true;

            //    SystemsManager.m_Camera.InitAngles();

            //    //SystemsManager.m_ScreenWipe.DoFade();
            //    m_camMenu.enabled = false;

            yield return new WaitForSeconds(0.2f);

            //    groupLevelSelect.gameObject.SetActive(false);
            //    groupTitle.gameObject.SetActive(false);
        //}
        }

        m_transitioning = false;
    }

    public void GoLevelSelect(bool reverse = false)
    {
        if (m_transitioning == false)
            StartCoroutine("MenuTransitionLevelSelect", reverse);
    }

    public void StartLevel()
    {
        //m_fader.color = new Color(0, 0, 0, 0);

        //groupHUD.gameObject.SetActive(true);

        //m_animator.SetTrigger("FadeInHUD");

        ////hud_minimap.enabled = true;
    }

    public void ShowGameOverScreen()
    {
        //if (SystemsManager.m_Game.m_wonTheGame)
        //{
        //    groupHUD.gameObject.SetActive(false);
        //    groupGameOver.gameObject.SetActive(true);

        //    ChangeGameOverStats(SystemsManager.m_Score.GetFullRunCoins().ToString(), SystemsManager.m_Timer.GetTotalTime().ToString());

        //    m_animator.SetTrigger("FadeInGameOver");
        //}
    }

    public void ShowLevelRecord()
    {
        //groupHUD.gameObject.SetActive(false);
        //groupGameOver.gameObject.SetActive(true);

        //ChangeGameOverStats(SystemsManager.m_Score.GetLevelCoins().ToString(), SystemsManager.m_Timer.GetTotalTime().ToString());

        //m_animator.SetTrigger("FadeInGameOver");
    }

    public void ToggleLevelIntro(bool toggle)
    {
        //if (toggle == true)
        //{
        //    for (int i = 0; i < intr_Key_Labels.Count; i++)
        //    {
        //        if (SystemsManager.m_Game.m_levelCurrent.getKeysRequired() > i)
        //        {
        //            intr_Key_Labels[i].enabled = toggle;
        //            hud_Key_Labels[i].enabled = toggle;
        //        }
        //    }

        //    m_animator.SetTrigger("FadeInIntro");
        //}
    }

    public void ToggleLevelOutro(bool toggle)
    {
        //groupHUD.gameObject.SetActive(false);
        ////hud_minimap.enabled = false;

        //outr_Time.text = SystemsManager.m_Timer.GetTime().ToString("##.##");
        //outr_Score.text = SystemsManager.m_Score.GetLevelCoins().ToString();
        //outr_Score.text += " / ";
        //outr_Score.text += SystemsManager.m_Game.m_levelCurrent.getGuffsCount().ToString();
        //outr_Time_Shadow.text = outr_Time.text;
        //outr_Score_Shadow.text = outr_Score.text;

        //groupOutro.gameObject.SetActive(toggle);

        //if (toggle == true)
        //    m_animator.SetTrigger("FadeInOutro");
    }

    public int getMenuSelector()
    {
        return m_menuSelector;
    }

    public IEnumerator ChangeMenuSelector(string dir)
    {
        m_events.SetSelectedGameObject(null);

        if (dir == "up")
        {
            if (m_state == MenuState.MainMenu)
            {
                if (m_menuSelector == 0)
                {
                    m_menuSelector = 3;
                }
                else if (m_menuSelector == 3)
                {
                    m_menuSelector = 0;
                }
                m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector -= 6;
                if (m_levelSelector < 0)
                    m_levelSelector += 12;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }
        else if (dir == "down")
        {
            if (m_state == MenuState.MainMenu)
            {
                if (m_menuSelector == 0)
                {
                    m_menuSelector = 3;
                }
                else if (m_menuSelector == 3)
                {
                    m_menuSelector = 0;
                }
                m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector += 6;
                if (m_levelSelector > 11)
                    m_levelSelector -= 12;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }
        else if (dir == "left")
        {
            if (m_state == MenuState.MainMenu)
            {
                //if (m_menuSelector == 0)
                //{
                //    m_menuSelector = 2;
                //}
                //else if (m_menuSelector == 1)
                //{
                //    m_menuSelector = 0;
                //}
                //else if (m_menuSelector == 2)
                //{
                //    m_menuSelector = 1;
                //}
                //m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector--;
                if (m_levelSelector < 0)
                    m_levelSelector += 12;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }
        else if (dir == "right")
        {
            if (m_state == MenuState.MainMenu)
            {
                //if (m_menuSelector == 0)
                //{
                //    m_menuSelector = 1;
                //}
                //else if (m_menuSelector == 1)
                //{
                //    m_menuSelector = 2;
                //}
                //else if (m_menuSelector == 2)
                //{
                //    m_menuSelector = 0;
                //}
                //m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector++;
                if (m_levelSelector > 11)
                    m_levelSelector -= 12;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }

        yield return null;
    }

    public void Quit()
    {
        Application.Quit();
    }
}