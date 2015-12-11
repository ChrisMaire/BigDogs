    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator m_animator;

    public enum CharacterState
    {
        Idle,
        Accel,
        LaneChange
    }

    [Header("State")]
    
    public CharacterState m_State;
    public bool m_finishedLevel;

    [Header("Collision")]
    
    private Rigidbody m_body;
    //private CharacterController m_controller;
    private CapsuleCollider m_collider;
    public bool m_grounded;
    public float m_groundCheckLength = 0.3f;
    public bool m_occupiedUp;
    public bool m_occupiedDown;

    private string m_layerRamp = "Ramp";
    private string m_layerGround = "Ground";

    [Header("Movement")]

    //public Vector3 m_move;
    public float m_accel = 0.15f;
    public float m_moveSpeed;
    public float m_moveFriction = 0.05f;
    public float m_maxSpeed = 5f;
    public float m_maxSpeedTotal = 9f;

    public bool m_onClearIce = true;
    public float m_badIcePenaltyMultiplier = 0.6f;

    public bool m_moveChangeReady = false;
    [Range(0.1f, 0.55f)]
    public float m_moveChangeAgainTime = 0.15f;
    private float m_moveChangeAgainTimer;

    public float m_gravity;
    public float m_gravityGround = -0.7f;
    public float m_gravityMultiplier = 0.9f;
    public float m_gravityMax = -10f;

    public float m_rampBoost = 1.5F;
    public bool m_ramping = false;
    
    public float m_jump = 8.0F;
    [Range(0.15f, 2.0f)]
    public float m_jumpAgainTime = 0.5f;
    private float m_jumpAgainTimer;
    public bool m_jumpReady;
    
    public float m_speedTurbo = 20.0F;

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
    public float m_laneChangePenaltyMultiplier = 0.75f;
    public float m_obstacleHitPenaltyMultiplier = 0.65f;
    public bool m_canShiftUp = true;
    public bool m_canShiftDown = true;

    private int m_tilesPassed;

    public Lane.LaneNumbers m_laneCurrent = Lane.LaneNumbers.Two;
        
    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        //m_controller = GetComponent<CharacterController>();
        m_collider = GetComponent<CapsuleCollider>();
        m_body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CorrectZDepth();

        if (m_finishedLevel == false)
        {
            m_tilesPassed = 1 + (int) ((transform.position.x + 2)/4);
            int lane = (int) m_laneCurrent;
            Lane.LaneType laneType = (Lane.LaneType) SystemsManager.m_Game.m_currentLevel.m_lanes[lane][m_tilesPassed];
            if (laneType == Lane.LaneType.Patchy)
            {
                m_onClearIce = false;
            }
            else if (laneType == Lane.LaneType.FinishLine)
            {
                m_finishedLevel = true;
            }
            else
            {
                m_onClearIce = true;
            }

            RaycastCheck();

            if (m_grounded)
            {
                if (SystemsManager.m_Input.inp_Push) // && m_speedState != Speed.Gallop)
                {
                    //ChangeTopSpeed();
                    m_moveSpeed += m_accel;
                    if (m_moveSpeed > m_maxSpeed)
                        m_moveSpeed = m_maxSpeed;
                }

                if (m_onClearIce == false)
                    m_moveSpeed *= m_badIcePenaltyMultiplier;

                m_moveSpeed -= m_moveFriction;

                if (m_laneChangeReady)
                {
                    CheckForLaneChange();
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

                m_gravity = m_gravityGround;

                CheckForJump();

                //LANE CORRECTION
                if (m_laneCurrent == Lane.LaneNumbers.One && m_laneDepthCurrent != m_laneDepth1)
                    m_laneDepthCurrent = m_laneDepth1;
                else if (m_laneCurrent == Lane.LaneNumbers.Two && m_laneDepthCurrent != m_laneDepth2)
                    m_laneDepthCurrent = m_laneDepth2;
                else if (m_laneCurrent == Lane.LaneNumbers.Three && m_laneDepthCurrent != m_laneDepth3)
                    m_laneDepthCurrent = m_laneDepth3;
                else if (m_laneCurrent == Lane.LaneNumbers.Four && m_laneDepthCurrent != m_laneDepth4)
                    m_laneDepthCurrent = m_laneDepth4;
            }

            //TURBO
            if (SystemsManager.m_Input.inp_Turbo)
                m_moveSpeed += m_speedTurbo;
        }
        else
        {
            m_moveSpeed -= m_moveFriction*5;
        }

        if (m_moveSpeed > m_maxSpeedTotal)
            m_moveSpeed = m_maxSpeedTotal;
        else if (m_moveSpeed < 0)
            m_moveSpeed = 0;

        if (m_grounded == false && m_ramping == false)
        {
            m_gravity += Physics.gravity.y * m_gravityMultiplier;
            if (m_gravity < m_gravityMax)
                m_gravity = m_gravityMax;
        }

        Vector3 force = (transform.up * m_gravity) + (transform.right * m_moveSpeed); ;

        //Debug.Log("velocity is " + m_body.velocity);
        //Debug.Log("adding force " + force);
        
        m_body.AddForce(force);
    }

    private void CorrectZDepth()
    {
        if (transform.position.z != m_laneDepthCurrent) //Reset Z Axis should it change for whatever reason
        {
            Debug.Log("resetting z");
            Vector3 tempVect = transform.position;
            tempVect.z = m_laneDepthCurrent;
            m_body.MovePosition(tempVect);
            transform.position = tempVect;
        }
    }

    private void RaycastCheck()
    {
        m_ramping = false;
        m_grounded = false;

        RaycastDown();
        RaycastSides();
    }

    private void RaycastDown()
    {
        Vector3 checkFrom = transform.position;
        Vector3 checkTo = -transform.up*m_groundCheckLength;
        var mask = (1 << LayerMask.NameToLayer("Ramp")) | (1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(checkFrom, checkTo, Color.magenta);
        RaycastHit hit;
        if (Physics.Raycast(checkFrom, checkTo, out hit, m_groundCheckLength, mask))
        {
            //Debug.Log("hit " + hit.collider.name + ", " + hit.collider.gameObject.layer);
            if (hit.collider.name != "GroundCollider")
            {
                //Debug.Log("rampo!");
                m_ramping = true;
                m_moveSpeed += m_rampBoost;
            }
            else
            {
                //Debug.Log("g-round");
                m_grounded = true;
            }
        }
    }
    private void RaycastSides()
    {
        Vector3 checkFrom = transform.position;
        Vector3 checkTo = transform.forward;
        var mask = (1 << LayerMask.NameToLayer("Ramp")) | (1 << LayerMask.NameToLayer("Obstacle"));
        RaycastHit hit;

        Debug.DrawRay(checkFrom, checkTo, Color.cyan);
        if (Physics.Raycast(checkFrom, checkTo, out hit, 1, mask))
        {
            Debug.Log("Deteceted a thing on the next lane up");
            m_canShiftUp = false;
        }
        else
        {
            m_canShiftUp = true;
        }

        Debug.DrawRay(checkFrom, -checkTo, Color.cyan);
        if (Physics.Raycast(checkFrom, -checkTo, out hit, 1, mask))
        {
            Debug.Log("Deteceted a thing on the next lane down");
            m_canShiftDown = false;
        }
        else
        {
            m_canShiftDown = true;
        }
    }

    void OnColliderEnter(Collision collision)
    {
        foreach (ContactPoint hit in collision.contacts)
        {
            Debug.Log("collider hit " + hit.otherCollider.gameObject.name);
        }
    }

    private void HitObstacle()
    {
        Debug.Log("lose mega-time");
    }

    private void CheckForLaneChange()
    {
        var ChangedLane = true;
        if (SystemsManager.m_Input.inp_D_Up && m_canShiftUp)
        {
            if (m_laneCurrent == Lane.LaneNumbers.One)
            {
                m_laneCurrent = Lane.LaneNumbers.Two;
            }
            else if (m_laneCurrent == Lane.LaneNumbers.Two)
            {
                m_laneCurrent = Lane.LaneNumbers.Three;
            }
            else if (m_laneCurrent == Lane.LaneNumbers.Three)
            {
                m_laneCurrent = Lane.LaneNumbers.Four;
            }
            else // no lane change possible
                ChangedLane = false;
        }
        else if (SystemsManager.m_Input.inp_D_Down && m_canShiftDown)
        {
            if (m_laneCurrent == Lane.LaneNumbers.Four)
            {
                m_laneCurrent = Lane.LaneNumbers.Three;
            }
            else if (m_laneCurrent == Lane.LaneNumbers.Three)
            {
                m_laneCurrent = Lane.LaneNumbers.Two;
            }
            else if (m_laneCurrent == Lane.LaneNumbers.Two)
            {
                m_laneCurrent = Lane.LaneNumbers.One;
            }
            else // no lane change possible
                ChangedLane = false;
        }
        else // no input
            ChangedLane = false;

        if (ChangedLane)
        {
            m_laneChangeReady = false;

            if (m_laneChangePenalty)
                m_moveSpeed *= m_laneChangePenaltyMultiplier;
        }
    }

    private void CheckForJump()
    {
        if (m_jumpReady)
        {
            if (SystemsManager.m_Input.inp_Jump)
            {
                Jump();
            }
        }
        else
        {
            //bunny hop prevention
            if (m_jumpAgainTimer < m_jumpAgainTime)
                m_jumpAgainTimer += Time.deltaTime;
            else
            {
                m_jumpReady = true;
                m_jumpAgainTimer = 0;
            }
        }
    }

    private void Jump()
    {
        m_gravity += m_jump;
        SystemsManager.m_SoundFX.OneShot_Jump();

        m_jumpReady = false;
    }
}