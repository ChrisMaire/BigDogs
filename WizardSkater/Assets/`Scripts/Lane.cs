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
                m_sprite.sprite = SystemsManager.m_Sprites.m_LanePatchy[(int)m_number+1];
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
                m_sprite.sprite = SystemsManager.m_Sprites.m_FinishLine;
                //LayFinishLineTile();
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
        //tempVect.y += 1;
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
        GameObject obstacleCollider = Instantiate(SystemsManager.m_Prefabs.m_obstacleCollider);
        Vector3 spriteVect = obstacleCollider.transform.position;
        obstacleCollider.name = "Obstacle";

        Vector3 tempVect = transform.position;
        //tempVect.z += 0.1f;
        obstacleCollider.transform.position = tempVect;
        obstacleCollider.transform.parent = transform.parent;

        GameObject obstacleSprite = Instantiate(SystemsManager.m_Prefabs.m_empty);
        obstacleSprite.name = "Sprite";
        obstacleSprite.transform.parent = obstacleCollider.transform;
        int rando = Random.Range(0, SystemsManager.m_Sprites.m_Obstacle.Count);
        obstacleSprite.GetComponent<SpriteRenderer>().sprite = SystemsManager.m_Sprites.m_Obstacle[rando];
        spriteVect.y += m_obstacleSpriteOffset;
        obstacleSprite.transform.localPosition = spriteVect;
        
    }

    private void Update()
    {
        
    }
}
