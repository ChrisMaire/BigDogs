using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class DebugScreenshot : MonoBehaviour
{
    public static int sN = 0;
    public string date;
    string year;
    string month;
    string day;

    // Use this for initialization
    void Start()
    {
        year = System.DateTime.Today.Year.ToString();
        //		year = Regex.Replace(date, @"[^\d]", "");
        month = System.DateTime.Today.Month.ToString().PadLeft(2);
        //		month = Regex.Replace(date, @"[^\d]", "");
        day = System.DateTime.Today.Day.ToString().PadLeft(2);
        //		day = Regex.Replace(date, @"[^\d]", "");

        date = year + month + day;
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug_Master.getStatus())
        {
            if (Input.GetMouseButtonUp(0))
            {
                SystemsManager.m_Game.setState(Game.GameState.Pause);

                Application.CaptureScreenshot("Screenshot - " + date + " - " + sN + ".png");
                sN++;

                SystemsManager.m_Game.setState(Game.GameState.Gameplay);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                SystemsManager.m_Game.setState(Game.GameState.Pause);

                Application.CaptureScreenshot("Screenshot - " + date + " - " + sN + ".png", 4);
                sN++;

                SystemsManager.m_Game.setState(Game.GameState.Gameplay);
            }
            else if (Input.GetMouseButtonUp(2))
            {
                SystemsManager.m_Game.setState(Game.GameState.Pause);

                Application.CaptureScreenshot("Screenshot - " + date + " - " + sN + ".png", 8);
                sN++;

                SystemsManager.m_Game.setState(Game.GameState.Gameplay);
            }
        }
    }

    public IEnumerator Screenshot()
    {
        SystemsManager.m_Game.setState(Game.GameState.Pause);

        yield return 1;

        Application.CaptureScreenshot("Screenshot - " + date + " - " + sN + ".png");
        sN++;

        yield return 90;

        SystemsManager.m_Game.setState(Game.GameState.Gameplay);
    }
}
