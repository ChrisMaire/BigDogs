using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level : MonoBehaviour
{
    public int m_lengths;

    protected GameObject m_ground;
    protected List<Lane> m_lanes;
    
    private void Start()
    {
        m_lanes = gameObject.GetComponentsInChildren<Lane>().ToList();
        for (int i = 0; i < m_lanes.Count; i++)
        {
            for (int j = -2; j < m_lengths+2; j++) //2 on each side for visibility
            {
                GameObject laneTile = Instantiate(SystemsManager.m_Prefabs.m_laneTile);
                laneTile.name = "Lane" + (i+1) + "." + j;
                
                Vector3 tempVect = m_lanes[i].transform.position;
                tempVect.x = (j - 1)*laneTile.transform.localScale.x;
                laneTile.transform.position = tempVect;

                laneTile.transform.parent = m_lanes[i].transform;
            }
        }

        m_ground = gameObject.transform.FindChild("Ground").gameObject;
        for (int i = -2; i < m_lengths+2; i++) //2 on each side for visibility
        {
            GameObject groundTile = Instantiate(SystemsManager.m_Prefabs.m_groundTile);
            groundTile.name = "Ground." + i;

            Vector3 tempVect = m_ground.transform.position;
            tempVect.x = (i - 1) * groundTile.transform.localScale.x;
            groundTile.transform.position = tempVect;

            groundTile.transform.parent = m_ground.transform;
        }
    }
}
