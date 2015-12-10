using UnityEngine; 
using System.Collections; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 
using System.Collections.Generic;
using System.Linq;

public class Level : MonoBehaviour
{
    private int m_lengths;
    protected GameObject m_ground;
    protected List<Lane> m_laneObjects;
    
    private string m_filePath;
    public string m_fileName = "Level1.xml";
    private string m_xmlString;

    private LevelData m_data;
    public List<List<int>> m_lanes;
    protected List<int> m_lane1;
    protected List<int> m_lane2;
    protected List<int> m_lane3;
    protected List<int> m_lane4;

    public BoxCollider m_collider;

    public void InitLevel()
    {
        //m_collider = GetComponentInChildren<BoxCollider>();

        m_lanes = new List<List<int>>();
        m_lane1 = new List<int>();
        m_lane2 = new List<int>();
        m_lane3 = new List<int>();
        m_lane4 = new List<int>();

        m_filePath = Application.dataPath;
        if(Application.platform == RuntimePlatform.OSXDashboardPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
        {
            m_filePath += "/Levels";
        }
        else
        {
            m_filePath += "\\Levels";
        }
        m_data = new LevelData();
        LoadXML();
        if (m_xmlString != "")
        {
            m_data = (LevelData)DeserializeObject(m_xmlString);
            m_lengths = m_data.Size;

            Vector3 tempVectPos = m_collider.transform.position;
            tempVectPos.x += ((m_lengths/2-4) * m_collider.size.x);
            m_collider.transform.position = tempVectPos;
            Vector3 tempVectSca = m_collider.size;
            tempVectSca.x *= m_lengths-2;
            m_collider.size = tempVectSca; 

            for (int i = 0; i < m_lengths; i++)
            {
                if (i < m_data.Lengths.Length)
                {
                    m_lane1.Add(m_data.Lengths[i].Lane1Contents);
                    m_lane2.Add(m_data.Lengths[i].Lane2Contents);
                    m_lane3.Add(m_data.Lengths[i].Lane3Contents);
                    m_lane4.Add(m_data.Lengths[i].Lane4Contents);
                }
                else
                {
                    m_lane1.Add(0);
                    m_lane2.Add(0);
                    m_lane3.Add(0);
                    m_lane4.Add(0);
                }
            }
            m_lanes.Add(m_lane1);
            m_lanes.Add(m_lane2);
            m_lanes.Add(m_lane3);
            m_lanes.Add(m_lane4);
            Debug.Log("level is go");
        }

        m_laneObjects = gameObject.GetComponentsInChildren<Lane>().ToList();
        LayTrackTiles();
        LayGroundTiles();
    }

    private void LayTrackTiles()
    {
        for (int i = 0; i < m_laneObjects.Count; i++)
        {
            for (int j = 0; j < m_lengths; j++) 
            {
                GameObject laneTile = Instantiate(SystemsManager.m_Prefabs.m_laneTile);
                laneTile.name = "Lane" + (i + 1) + "." + j;

                Vector3 tempVect = m_laneObjects[i].transform.position;
                tempVect.x = (j - 1)*laneTile.transform.localScale.x;
                laneTile.transform.position = tempVect;

                laneTile.transform.parent = m_laneObjects[i].transform;


                Lane lane = laneTile.GetComponent<Lane>();
                if (lane == null)
                    Debug.Log("lane is null!");
                else
                {
                    if (j >= 0 && j < m_lengths)
                    {
                        //Debug.Log("i= " + i + ", j = " + j + "| type = " + (Lane.LaneType) m_lanes[i][j]);
                        lane.m_number = (Lane.LaneNumbers)i;
                        lane.m_type = (Lane.LaneType) m_lanes[i][j];
                        lane.Init();
                    }
                    else
                    {
                        lane.Init();
                    }
                }
            }
        }
    }

    private void LayGroundTiles()
    {
        m_ground = gameObject.transform.FindChild("Ground").gameObject;
        for (int i = 0; i < m_lengths; i++) //2 on each side for visibility
        {
            GameObject groundTile = Instantiate(SystemsManager.m_Prefabs.m_groundTile);
            groundTile.name = "Ground." + i;

            Vector3 tempVect = groundTile.transform.position;
            tempVect.x = (i-1)*groundTile.transform.localScale.x;
            groundTile.transform.position = tempVect;

            groundTile.transform.parent = m_ground.transform;
        }
    }

    void LoadXML()
    {
        string file = m_filePath;
        if (Application.platform == RuntimePlatform.OSXDashboardPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
        {
            file += "/";
        }
        else
        {
            file += "\\";
        }
        file += m_fileName;
        StreamReader r = File.OpenText(file);
        string info = r.ReadToEnd();
        r.Close();
        m_xmlString = info;
    }

    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(LevelData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(LevelData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }
}

[System.Serializable]
public struct LaneData
{
    public int Number;
    public int Lane1Contents;
    public int Lane2Contents;
    public int Lane3Contents;
    public int Lane4Contents;
}

[System.Serializable]
public struct LevelData
{
    public int Size;
    public LaneData[] Lengths;
}