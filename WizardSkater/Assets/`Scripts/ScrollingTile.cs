using UnityEngine;
using System.Collections;

public class ScrollingTile : MonoBehaviour
{
    public float m_scrollMultiplier = 1.0f;
    private float m_scrollSpeedBase = 0.25f;
    private float m_scrollSpeed;
    private float m_offset;
    private float m_rotate;
    private Rigidbody m_targetToTrack;
    private Renderer m_renderer;

    void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }
 
    void Update()
    {
        if (SystemsManager.m_Game.getState() != Game.GameState.Create)
        {
            if (m_targetToTrack == null)
                m_targetToTrack = SystemsManager.m_Player.GetComponent<Rigidbody>();

            m_scrollSpeed = m_scrollSpeedBase * m_scrollMultiplier * m_targetToTrack.velocity.x;
            m_offset += (Time.deltaTime * m_scrollSpeed) / 10.0f;
            m_renderer.material.SetTextureOffset("_MainTex", new Vector2(m_offset, 0));
        }

        

    }
}
