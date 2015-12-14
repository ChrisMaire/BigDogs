using UnityEngine;
using System.Collections;

public class Camera_Wizard : MonoBehaviour
{
    private GameObject m_player;
    public Vector3 m_followDistance;
    public float m_followSpeed;

    private void LateUpdate()
    {
        if (m_player == null)
        {
            m_player = SystemsManager.m_Player.gameObject;
            if (m_player == null)
            {
                Debug.Log("where is pretty baby???");
            }
        }

        if ((SystemsManager.m_Game.getState() == Game.GameState.Testing || SystemsManager.m_Game.getState() == Game.GameState.Gameplay ||
            SystemsManager.m_Game.getState() == Game.GameState.LevelComplete ||
            SystemsManager.m_Game.getState() == Game.GameState.PostOutro ||
            SystemsManager.m_Game.getState() == Game.GameState.LevelIntro) &&
             m_player != null)
        {
            var target = m_player.transform.position + m_followDistance;
            target.z = m_followDistance.z;
            target.y = m_followDistance.y;
            transform.position = Vector3.Lerp(transform.position,target,m_followSpeed);
        }
    }
}
