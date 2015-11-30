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

    public bool m_hitObject;
    private bool m_grounded;

    [Header("Jump")]

    public float m_jump = 8.0F;
    [Range(0.15f, 2.0f)] public float m_jumpAgainTime = 0.5f;
    private float m_jumpAgainTimer;
    private bool m_jumpReady;

    [Header("Speed")]
    
    public float m_moveSpeedCurrent = 0.0F;
    public float m_moveSpeedAccel = 0.5F;
    public bool m_moveChangeReady = false;
    [Range(0.1f, 0.55f)] public float m_moveChangeAgainTime = 0.15f;
    private float m_moveChangeAgainTimer;
    public float m_moveSpeed1 = 5.0F;
    public float m_moveSpeed2 = 10.0F;
    public float m_moveSpeed3 = 15.0F;
    public bool m_TurboOnC = false;
    public float m_speedTurbo = 20.0F;

    public enum Speed
    {
        Idle,
        Trot,
        Canter,
        Gallop
    }
    public Speed m_speedCurrent = Speed.Idle;

    [Header("Lanes")]

    public float m_laneDepthCurrent;
    public float m_laneDepth1;
    public float m_laneDepth2;
    public float m_laneDepth3;
    public float m_laneDepth4;
    public bool m_laneChangeReady = false;
    [Range(0.05f, 0.35f)] public float m_laneChangeAgainTime = 0.1f;
    private float m_laneChangeAgainTimer;
    public bool m_laneChangePenalty = false;
    public float m_speedLossLaneChange = 5.0F;

    public enum Lanes
    {
        Lane1,
        Lane2,
        Lane3,
        Lane4
    }
    public Lanes m_laneCurrent = Lanes.Lane2;
    
    /// CORE
    
    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        //CORE MOVEMENT
        var tempVect = transform.position;
        tempVect.x += (m_moveSpeedCurrent*Time.deltaTime);
        tempVect.y += (m_gravity*Time.deltaTime);
        tempVect.z = m_laneDepthCurrent;
        transform.position = tempVect;

        if (m_moveChangeReady)
        {
            if (SystemsManager.m_Input.inp_Push && m_speedCurrent != Speed.Gallop)
            {
                //SPEED & ACCELERATION
                if (m_speedCurrent == Speed.Idle && m_moveSpeedCurrent <= 0f ||
                    m_speedCurrent == Speed.Trot && m_moveSpeedCurrent <= m_moveSpeed1 ||
                    m_speedCurrent == Speed.Canter && m_moveSpeedCurrent <= m_moveSpeed2 ||
                    m_speedCurrent == Speed.Gallop && m_moveSpeedCurrent <= m_moveSpeed3)
                {
                    //SPEED CHANGE
                    Debug.Log("push " + m_speedCurrent);
                    if (m_speedCurrent == Speed.Idle)
                        m_speedCurrent = Speed.Trot;
                    else if (m_speedCurrent == Speed.Trot)
                        m_speedCurrent = Speed.Canter;
                    else if (m_speedCurrent == Speed.Canter)
                        m_speedCurrent = Speed.Gallop;

                    m_moveChangeReady = false;
                }
            }
        }
        else
        {
            //lane spam prevention
            if (m_moveChangeAgainTimer < m_moveChangeAgainTime)
                m_moveChangeAgainTimer += Time.deltaTime;
            else if (SystemsManager.m_Input.inp_D_Up == false && SystemsManager.m_Input.inp_D_Down == false)
            {
                m_moveChangeReady = true;
                m_moveChangeAgainTimer = 0;
            }
        }

        m_moveSpeedCurrent += m_moveSpeedAccel;

        if (m_speedCurrent == Speed.Trot && m_moveSpeedCurrent > m_moveSpeed1)
            m_moveSpeedCurrent = m_moveSpeed1;
        if (m_speedCurrent == Speed.Canter && m_moveSpeedCurrent > m_moveSpeed2)
            m_moveSpeedCurrent = m_moveSpeed2;
        if (m_speedCurrent == Speed.Gallop && m_moveSpeedCurrent > m_moveSpeed3)
            m_moveSpeedCurrent = m_moveSpeed3;
        if (m_moveSpeedCurrent <= 0f && m_speedCurrent != Speed.Idle)
        {
            m_speedCurrent = Speed.Idle;
            m_moveChangeReady = true;
        }
        if (m_speedCurrent == Speed.Idle)
            m_moveSpeedCurrent = 0f;

        //LANE CORRECTION
        if (m_laneCurrent == Lanes.Lane1 && m_laneDepthCurrent != m_laneDepth1)
            m_laneDepthCurrent = m_laneDepth1;
        else if (m_laneCurrent == Lanes.Lane2 && m_laneDepthCurrent != m_laneDepth2)
            m_laneDepthCurrent = m_laneDepth2;
        else if (m_laneCurrent == Lanes.Lane3 && m_laneDepthCurrent != m_laneDepth3)
            m_laneDepthCurrent = m_laneDepth3;
        else if (m_laneCurrent == Lanes.Lane4 && m_laneDepthCurrent != m_laneDepth4)
            m_laneDepthCurrent = m_laneDepth4;

        //LANE CHANGE
        if (m_laneChangeReady)
        {
            if(SystemsManager.m_Input.inp_D_Up)
            { 
            Debug.Log("Lane Change up");
            var ChangedLane = true;
            if (m_laneCurrent == Lanes.Lane1)
                m_laneCurrent = Lanes.Lane2;
            else if (m_laneCurrent == Lanes.Lane2)
                m_laneCurrent = Lanes.Lane3;
            else if (m_laneCurrent == Lanes.Lane3)
                m_laneCurrent = Lanes.Lane4;
            else
                ChangedLane = false;

            if (ChangedLane)
                m_laneChangeReady = false;
            }
            else if (SystemsManager.m_Input.inp_D_Down)
            {
                Debug.Log("Lane Change down");
                var ChangedLane = true;
                if (m_laneCurrent == Lanes.Lane4)
                    m_laneCurrent = Lanes.Lane3;
                else if (m_laneCurrent == Lanes.Lane3)
                    m_laneCurrent = Lanes.Lane2;
                else if (m_laneCurrent == Lanes.Lane2)
                    m_laneCurrent = Lanes.Lane1;
                else
                    ChangedLane = false;

                if (ChangedLane)
                    m_laneChangeReady = false;
            }
        }
        else
        {
            //lane spam prevention
            if (m_laneChangeAgainTimer < m_laneChangeAgainTime)
                m_laneChangeAgainTimer += Time.deltaTime;
            else if (SystemsManager.m_Input.inp_D_Up == false && SystemsManager.m_Input.inp_D_Down == false)
            {
                m_laneChangeReady = true;
                m_laneChangeAgainTimer = 0;
            }
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

    //FUNCTIONS


}