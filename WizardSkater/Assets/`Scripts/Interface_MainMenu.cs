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

    public CanvasGroup groupTitle;
    public CanvasGroup groupLevelSelect;
    public CanvasGroup groupAttractMode;

    public enum MenuState
    {
        AttractMode,
        MainMenu,
        LevelSelect
    };

    public MenuState m_state;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        SystemsManager.m_Score.Init();

        m_state = MenuState.MainMenu;  
        m_menuSelectButtons[0].Select();
    }

    void Update()
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
        else
        {
            if (SystemsManager.m_Input.inp_Jump)
            {
                if (getMenuSelector() == 0 && SystemsManager.m_Game.m_startedTheGame == false)
                {
                    GoLevelSelect(false);
                }
                else if (getMenuSelector() == 1)
                {
                    Application.Quit();
                }
            }
            else
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

    public IEnumerator MenuTransitionMainMenu(bool reverse)
    {
        m_transitioning = true;

        m_animator.ResetTrigger("FadeOutMainMenu");
        m_animator.ResetTrigger("FadeOutLevelSelect");
        m_animator.ResetTrigger("FadeOutHow");

        if (reverse == false)
        {
            if (groupLevelSelect.gameObject)
            {
                //Debug.Log("mm in");

                groupTitle.gameObject.SetActive(true);
                groupTitle.alpha = 0f;
                m_menuSelectButtons[m_menuSelector].Select();

                m_animator.SetTrigger("LevelSelectOut");

                yield return new WaitForSeconds(0.2f);

                groupLevelSelect.alpha = 0f;
                groupLevelSelect.gameObject.SetActive(false);

                groupTitle.alpha = 1f;
                m_state = MenuState.MainMenu;
            }
        }
        else
        {
            Debug.Log("mm out");
            m_animator.SetTrigger("LevelSelectIn");

            yield return new WaitForSeconds(0.2f);

            //yield return new WaitForSeconds(0.2f);

            groupTitle.gameObject.SetActive(false);
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

        //m_animator.ResetTrigger("FadeOutMainMenu");
        //m_animator.ResetTrigger("FadeOutLevelSelect");
        //m_animator.ResetTrigger("FadeOutHow");

        if (reverse == false)
        {
            Debug.Log("ls in");
            groupLevelSelect.gameObject.SetActive(true);
            groupLevelSelect.alpha = 0f;
            m_levelSelectButtons[0].Select();

            for (int i = 0; i < m_levelSelectButtons.Length; i++) //buttons
            {
                m_levelSelectButtons[i].enabled = true;
            }

            //for (int i = 0; i < m_levelSelectBestTimes.Count - 1; i += 1) //best times
            //{
            //    float top = SystemsManager.m_Score.m_levelTopScores[i];
            //    string topS = top.ToString("0.00");
            //    if (SystemsManager.m_Score.m_levelTopScores.Count >= i && top != -1)
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

            m_animator.SetTrigger("LevelSelectIn");

            yield return new WaitForSeconds(0.2f);

            groupTitle.alpha = 0f;
            groupTitle.gameObject.SetActive(false);

            groupLevelSelect.alpha = 1f;
            m_state = MenuState.LevelSelect;
        }
        else
        {
            Debug.Log("ls out");
            m_animator.SetTrigger("LevelSelectOut");

            yield return new WaitForSeconds(0.2f);

            groupLevelSelect.alpha = 0f;

            if (SystemsManager.m_Game.m_currentLevel == null)
            {
                StartCoroutine("MenuTransitionMainMenu", false);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);

                groupLevelSelect.gameObject.SetActive(false);
                groupTitle.gameObject.SetActive(false);
            }
        }

        m_transitioning = false;
    }

    public void GoLevelSelect(bool reverse = false)
    {
        if (m_transitioning == false)
            StartCoroutine("MenuTransitionLevelSelect", reverse);
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
                //if (m_menuSelector == 0)
                //{
                //    m_menuSelector = 3;
                //}
                //else if (m_menuSelector == 3)
                //{
                //    m_menuSelector = 0;
                //}
                //m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector -= 2;
                if (m_levelSelector < 0)
                    m_levelSelector += 4;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }
        else if (dir == "down")
        {
            if (m_state == MenuState.MainMenu)
            {
                //if (m_menuSelector == 0)
                //{
                //    m_menuSelector = 3;
                //}
                //else if (m_menuSelector == 3)
                //{
                //    m_menuSelector = 0;
                //}
                //m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector += 2;
                if (m_levelSelector > 4)
                    m_levelSelector -= 4;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }
        else if (dir == "left")
        {
            if (m_state == MenuState.MainMenu)
            {
                if (m_menuSelector == 0)
                {
                    m_menuSelector = 1;
                }
                else if (m_menuSelector == 1)
                {
                    m_menuSelector = 0;
                }
                //else if (m_menuSelector == 2)
                //{
                //    m_menuSelector = 1;
                //}
                m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector--;
                if (m_levelSelector < 0)
                    m_levelSelector += 4;
                m_levelSelectButtons[m_levelSelector].Select();
            }
        }
        else if (dir == "right")
        {
            if (m_state == MenuState.MainMenu)
            {
                if (m_menuSelector == 0)
                {
                    m_menuSelector = 1;
                }
                else if (m_menuSelector == 1)
                {
                    m_menuSelector = 0;
                }
                //else if (m_menuSelector == 2)
                //{
                //    m_menuSelector = 0;
                //}
                m_menuSelectButtons[m_menuSelector].Select();
            }
            else if (m_state == MenuState.LevelSelect)
            {
                m_levelSelector++;
                if (m_levelSelector > 4)
                    m_levelSelector -= 4;
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