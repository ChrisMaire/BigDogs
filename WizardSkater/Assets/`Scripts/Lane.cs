using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour
{
    public SpriteRenderer m_sprite;

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

    public LaneType m_type;

    public void Init(LaneType type)
    {
        m_type = type;
        m_sprite = GetComponent<SpriteRenderer>();

        switch (m_type)
        {
            case LaneType.Patchy:
            {
            m_sprite.sprite = SystemsManager.m_Sprites.m_LanePatchy;
            }
            break;
            case LaneType.Obstacle:
            {
            m_sprite.sprite = SystemsManager.m_Sprites.m_LaneObstacle;
            }
            break;
            case LaneType.Ramp:
            {
            m_sprite.sprite = SystemsManager.m_Sprites.m_LaneRamp;
            }
            break;
            case LaneType.FinishLine:
            {
            m_sprite.sprite = SystemsManager.m_Sprites.m_LaneFinishLine;
            }
            break;
            default:
            {
            int rando = Random.Range(0, SystemsManager.m_Sprites.m_LaneNormal.Count);
            m_sprite.sprite = SystemsManager.m_Sprites.m_LaneNormal[rando];
            }
            break;
        }
    }

    private void Update()
    {
        
    }
}
