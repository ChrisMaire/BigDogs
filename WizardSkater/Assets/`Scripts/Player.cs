using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Accel,
        LaneChange
    }
    public CharacterState m_State;

    private Animator m_animator;

    [Header("Movement")]

    public float m_gravityGround = -0.7f;
    private float m_gravity;
    public float m_gravityMultiplier = 0.9f;

    [Header("Collision")]

    private bool m_grounded;
    public bool m_hitObject;

    [Header("Jump")]

    public float m_jump = 8.0F;
    [Range(0.15f, 2.0f)] public float m_jumpAgainTime = 0.5f;
    private float m_jumpAgainTimer;
    private bool m_jumpReady;

    [Header("Speed")]

    private bool m_pushed = false;
    public float m_moveSpeedCurrent = 0.0F;
    public enum Speed
    {
        Idle,
        Trot,
        Canter,
        Gallop
    }
    private Speed m_speedCurrent = Speed.Idle;
    public float m_moveSpeedAccel = 0.5F;
    public bool m_moveChangeReady = false;
    public float m_moveSpeed1 = 5.0F;
    public float m_moveSpeed2 = 10.0F;
    public float m_moveSpeed3 = 15.0F;
    public bool m_TurboOnC = false;
    public float m_speedTurbo = 20.0F;

    [Header("LaneChange")]

    public bool m_laneChangeReady = false;
    public bool m_laneChangePenalty = false;
    public float m_speedLossLaneChange = 5.0F;
    
    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        //CORE MOVEMENT
        transform.position = new Vector3(transform.position.x + m_moveSpeedCurrent, transform.position.y + m_gravity, transform.position.z);

        if (m_speedCurrent != Speed.Gallop)
        {
            if (SystemsManager.m_Input.inp_Push && !m_pushed)
            {
                //SPEED & ACCELERATION
                if (m_speedCurrent == Speed.Trot && m_moveSpeedCurrent < m_moveSpeed1 ||
                    m_speedCurrent == Speed.Canter && m_moveSpeedCurrent < m_moveSpeed2 ||
                    m_speedCurrent == Speed.Gallop && m_moveSpeedCurrent < m_moveSpeed3)
                {
                    m_moveSpeedCurrent += m_moveSpeedAccel;

                    if (m_speedCurrent == Speed.Trot && m_moveSpeedCurrent > m_moveSpeed1)
                        m_moveSpeedCurrent = m_moveSpeed1;
                    if (m_speedCurrent == Speed.Canter && m_moveSpeedCurrent > m_moveSpeed2)
                        m_moveSpeedCurrent = m_moveSpeed2;
                    if (m_speedCurrent == Speed.Gallop && m_moveSpeedCurrent > m_moveSpeed3)
                        m_moveSpeedCurrent = m_moveSpeed3;

                    if(SystemsManager.m_Input.inp_Push && m_moveChangeReady == true)
                    {
                        if (m_speedCurrent == Speed.Idle)
                            m_speedCurrent = Speed.Trot;
                        else if (m_speedCurrent == Speed.Trot)
                            m_speedCurrent = Speed.Canter;
                        else if (m_speedCurrent == Speed.Canter)
                            m_speedCurrent = Speed.Gallop;
                    }
                }
                else if (m_moveSpeedCurrent <= 0f && m_speedCurrent != Speed.Idle)
                {
                    m_speedCurrent = Speed.Idle;
                    m_moveChangeReady = true;
                }

                //LANE CHANGE
                if (m_laneChangeReady && SystemsManager.m_Input.inp_D_Up)
                {
                    Debug.Log("Lane Change up");
                }
                if (m_laneChangeReady && SystemsManager.m_Input.inp_D_Down)
                {
                    Debug.Log("Lane Change down");
                }

                //JUMPING
                if (m_grounded)
                {
                    if (m_jumpReady)
                    {
                        if (SystemsManager.m_Input.inp_Jump)
                        {
                            m_gravity += m_jump;
                            //m_speedMax = m_speedAir;
                            SystemsManager.m_SoundFX.OneShot_Jump();

                            m_jumpReady = false;
                        }
                    }
                    else
                    {
                        m_gravity = m_gravityGround;

                        //bunny hop prevention
                        if (m_jumpAgainTimer < m_jumpAgainTime)
                            m_jumpAgainTimer += Time.deltaTime;
                        else if (SystemsManager.m_Input.inp_Jump == false)
                        {
                            m_jumpReady = true;
                            m_jumpAgainTimer = 0;
                        }
                    }
                }
                else ////////////////////FALLING
                {
                    //m_gravity += Physics.gravity.y * m_gravityMultiplier;
                }
            }
        }
    }
}