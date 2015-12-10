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

    [Header("State")]
    
    public CharacterState m_State;

    private Animator m_animator;

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

    public enum Lanes
    {
        Lane1,
        Lane2,
        Lane3,
        Lane4
    }
    public Lanes m_laneCurrent = Lanes.Lane2;
        
    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        //m_controller = GetComponent<CharacterController>();
        m_collider = GetComponent<CapsuleCollider>();
        m_body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.z != m_laneDepthCurrent)  //Reset Z Axis should it change for whatever reason
        {
            Debug.Log("resetting z");
            Vector3 tempVect = transform.position;
            tempVect.z = m_laneDepthCurrent;
            m_body.MovePosition(tempVect);
            transform.position = tempVect;
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

            m_moveSpeed -= m_moveFriction;
            if (m_moveSpeed < 0f)
                m_moveSpeed = 0f;

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
            if (m_laneCurrent == Lanes.Lane1 && m_laneDepthCurrent != m_laneDepth1)
                m_laneDepthCurrent = m_laneDepth1;
            else if (m_laneCurrent == Lanes.Lane2 && m_laneDepthCurrent != m_laneDepth2)
                m_laneDepthCurrent = m_laneDepth2;
            else if (m_laneCurrent == Lanes.Lane3 && m_laneDepthCurrent != m_laneDepth3)
                m_laneDepthCurrent = m_laneDepth3;
            else if (m_laneCurrent == Lanes.Lane4 && m_laneDepthCurrent != m_laneDepth4)
                m_laneDepthCurrent = m_laneDepth4;
        }
        else
        {
            if (m_ramping == false)
            {
                Debug.Log("falling at " + Physics.gravity.y * m_gravityMultiplier);
                m_gravity += Physics.gravity.y * m_gravityMultiplier;
                if (m_gravity < m_gravityMax)
                    m_gravity = m_gravityMax;
            }
        }

        //TURBO
        if (SystemsManager.m_Input.inp_Turbo)
            m_moveSpeed += m_speedTurbo;

        if (m_moveSpeed > m_maxSpeedTotal)
            m_moveSpeed = m_maxSpeedTotal;
        
        Vector3 force = (transform.up * m_gravity) + (transform.right * m_moveSpeed); ;

        Debug.Log("velocity is " + m_body.velocity);
        //Debug.Log("adding force " + force);
        m_body.AddForce(force);
    }

    private void RaycastCheck()
    {
        m_ramping = false;
        m_grounded = false;

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
        if (SystemsManager.m_Input.inp_D_Up)
        {
            if (m_laneCurrent == Lanes.Lane1)
            {
                m_laneCurrent = Lanes.Lane2;
            }
            else if (m_laneCurrent == Lanes.Lane2)
            {
                m_laneCurrent = Lanes.Lane3;
            }
            else if (m_laneCurrent == Lanes.Lane3)
            {
                m_laneCurrent = Lanes.Lane4;
            }
            else // no lane change possible
                ChangedLane = false;
        }
        else if (SystemsManager.m_Input.inp_D_Down)
        {
            if (m_laneCurrent == Lanes.Lane4)
            {
                m_laneCurrent = Lanes.Lane3;
            }
            else if (m_laneCurrent == Lanes.Lane3)
            {
                m_laneCurrent = Lanes.Lane2;
            }
            else if (m_laneCurrent == Lanes.Lane2)
            {
                m_laneCurrent = Lanes.Lane1;
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