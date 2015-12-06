using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour
{
    private SpriteRenderer m_sprite;

    public enum LaneNumbers
    {
        One,
        Two,
        Three,
        Four
    }

    public LaneNumbers m_number;

    public enum LaneType
    {
        Normal,
        Ramp,
        Patchy,
        Obstacle,
        FinishLine
    }

    public LaneType m_type = LaneType.Normal;

    public void Init()
    {
        m_sprite = GetComponentInChildren<SpriteRenderer>();

        m_sprite.sprite = SystemsManager.m_Sprites.m_LaneNormal[(int)m_number];
        
        switch (m_type)
        {
            case LaneType.Patchy:
            {
            m_sprite.sprite = SystemsManager.m_Sprites.m_LanePatchy;
            }
            break;
            case LaneType.Obstacle:
            {
                GameObject obstacle = Instantiate(SystemsManager.m_Prefabs.m_empty);
                obstacle.transform.parent = transform;
                Vector3 tempVect = transform.position;
                tempVect.y += 1;
                obstacle.transform.position = tempVect;
                obstacle.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_Obstacle;
            }
            break;
            case LaneType.Ramp:
            {
                GameObject ramp = Instantiate(SystemsManager.m_Prefabs.m_empty);
                ramp.transform.parent = transform;
                Vector3 tempVect = transform.position;
                tempVect.y += 1;
                ramp.transform.position = tempVect;
                ramp.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_Ramp;
            }
            break;
            case LaneType.FinishLine:
            {
                GameObject finishLine = Instantiate(SystemsManager.m_Prefabs.m_empty);
                finishLine.transform.parent = transform;
                Vector3 tempVect = transform.position;
                tempVect.y += 1;
                finishLine.transform.position = tempVect;
                finishLine.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_FinishLine;
            }
            break;
            default:
            {
            }
            break;
        }
    }

    private void Update()
    {
        
    }
}
