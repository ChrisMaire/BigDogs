using UnityEngine;
using System.Collections;

public class Camera_Wizard : MonoBehaviour
{
    private GameObject m_player;

    public void Init()
    {
        m_player = SystemsManager.m_Player.gameObject;
    }

    private void LateUpdate()
    {
        if ((SystemsManager.m_Game.getState() == Game.GameState.Testing || SystemsManager.m_Game.getState() == Game.GameState.Gameplay) &&
             m_player != null)
        {
            var target = m_player.transform.position;
        }
    }
}
