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

    public Vector3 m_rampSpriteOffset;
    public float m_obstacleSpriteOffset = 0.1f;

    public void Init()
    {
        m_sprite = GetComponentInChildren<SpriteRenderer>();

        //magic number :(
        m_rampSpriteOffset = new Vector3(0.65f, 0.775f, 0f);;

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
                LayObstacleTile();
            }
            break;
            case LaneType.Ramp:
            {
                LayRampTile();
            }
            break;
            case LaneType.FinishLine:
            {
                LayFinishLineTile();
            }
            break;
            default:
            {
            }
            break;
        }
    }

    private void LayFinishLineTile()
    {
        GameObject finishLine = Instantiate(SystemsManager.m_Prefabs.m_empty);
        finishLine.transform.parent = transform;
        Vector3 tempVect = transform.position;
        tempVect.y += 1;
        finishLine.transform.position = tempVect;
        finishLine.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_FinishLine;
    }

    private void LayRampTile()
    {
        GameObject rampCollider = Instantiate(SystemsManager.m_Prefabs.m_rampCollider);
        Vector3 spriteVect = rampCollider.transform.position;
        rampCollider.name = "Ramp";
        
        Vector3 tempVect = transform.position;
        //tempVect.y += 1.23f;
        //rampCollider.transform.position = tempVect;
        rampCollider.transform.position = tempVect;
        rampCollider.transform.parent = transform.parent;

        GameObject rampSprite = Instantiate(SystemsManager.m_Prefabs.m_empty);
        rampSprite.name = "Sprite";
        rampSprite.transform.parent = rampCollider.transform;
        rampSprite.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_Ramp;
        spriteVect += m_rampSpriteOffset;
        rampSprite.transform.localPosition = spriteVect;
    }

    private void LayObstacleTile()
    {
        GameObject obstacle = Instantiate(SystemsManager.m_Prefabs.m_empty);
        obstacle.name = "Obstacle";
        obstacle.transform.parent = transform;
        Vector3 tempVect = transform.position;
        tempVect.y += m_obstacleSpriteOffset;
        obstacle.transform.position = tempVect;
        obstacle.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_Obstacle;

        GameObject obstacleCollider = Instantiate(SystemsManager.m_Prefabs.m_obstacleCollider);
        Vector3 storedVect = obstacle.transform.position;
        obstacleCollider.name = "Collider";
        obstacleCollider.transform.position = tempVect;
        obstacleCollider.transform.parent = obstacle.transform;
        obstacleCollider.transform.localPosition = storedVect;
    }

    private void Update()
    {
        
    }
}
