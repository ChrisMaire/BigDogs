    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]

    public float m_blinkInterval = 0.1f;
    private SpriteRenderer m_renderer;
    private Animator m_animator;
    private bool m_animationChangeReady;
    private float m_animationChangeTimer;
    public float m_animationChangeTime = 1f;

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

    public bool m_inputFreeze;
    private float m_inputFreezeTimer;
    private float m_inputFreezeTime;
    public float m_accel = 0.15f;
    public float m_moveSpeed;
    public float m_moveFriction = 0.05f;
    public float m_maxSpeed = 5f;
    public float m_maxSpeedTotal = 9f;

    public float m_obstacleFreezeInputTime = 0.5f;
    public float m_obstacleHitPenaltyMultiplier = 0.25f;

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

    public bool m_tricking;
    [Range(0.5f, 2.0f)]
    public float m_trickTime = 1f;
    private float m_trickTimer;

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
    [Range(0.05f, 0.35f)]
    public float m_laneChangeAgainTime = 0.1f;
    private float m_laneChangeAgainTimer;
    public float m_laneChangePenaltyMultiplier = 0.75f;
    public bool m_canShiftUp = true;
    public bool m_canShiftDown = true;

    public Lane m_laneCurrentlyOn;

    private int m_tilesPassed;

    public Lane.LaneNumbers m_laneCurrent = Lane.LaneNumbers.Two;

    [Header("MagicBar")]

    public float m_magicCurrent = 0f;
    public float m_magicAmtPassive = 1f;
    public float m_magicAmtTrick = 10f;
    public float m_magicMax = 100f;
    public float m_magicAmtTurbo = 3f;
    public float m_magicAmtRamp = 45f;

    void Init()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_animator = GetComponentInChildren<Animator>();
        //m_controller = GetComponent<CharacterController>();
        m_collider = GetComponent<CapsuleCollider>();
        m_body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(m_animator == null || m_collider == null || m_body == null || m_renderer == null)
            Init();

        CorrectZDepth();

        if (m_finishedLevel == false)
        {
            if (m_inputFreeze)
            {
                if (m_inputFreezeTimer > m_inputFreezeTime)
                {
                    m_inputFreeze = false;
                    m_inputFreezeTimer = 0f;
                }
                else
                {
                    m_inputFreezeTimer += Time.fixedDeltaTime;
                }
            }

            CheckLaneType();

            CheckRaycastDirections();

            if (m_grounded)
            {
                GroundAnimationLogic();

                if (SystemsManager.m_Input.inp_Skate && !m_inputFreeze)
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

                MoveToCurrentLane();
            }
            else
            {
                if (m_animator.GetBool("grounded") == true)
                    m_animator.SetBool("grounded", false);

                //m_animator.speed = 0f;
                m_animator.ResetTrigger("skate");
                m_animator.ResetTrigger("idle");
            }

            CheckForTrick();

            if (m_tricking == false)
            { 
                CheckForTurbo();

                CheckForRampMagic();
            }

            UpdateMagic();
        }
        else
        {
            m_moveSpeed -= m_moveFriction*5;
        }


        ValidateSpeed();

        Falling();

        Vector3 force = (transform.up * m_gravity) + (transform.right * m_moveSpeed);
        
        m_body.AddForce(force);

        //Debug.Log("velocity is " + m_body.velocity);
        //Debug.Log("adding force " + force);
    }

    private void UpdateMagic()
    {
        if (m_magicCurrent < 0)
            m_magicCurrent = 0;

        if (m_magicCurrent < m_magicMax)
            m_magicCurrent += m_magicAmtPassive * Time.fixedDeltaTime;

        if (m_magicCurrent > m_magicMax)
            m_magicCurrent = m_magicMax;
    }

    private void CheckLaneType()
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
            FinishLevel();
        }
        else
        {
            m_onClearIce = true;
        }
    }

    private void FinishLevel()
    {
        m_finishedLevel = true;
        SystemsManager.m_Timer.SetTimePause(true);
        SystemsManager.m_Game.setState(Game.GameState.LevelComplete);
        SystemsManager.m_Score.EvaluateLevelHighScore(SystemsManager.m_Game.m_currentLevel.m_levelOrder - 1);
        SystemsManager.m_interGame.StartCoroutine("LevelComplete");
    }

    private void CheckForTurbo()
    {
        if (SystemsManager.m_Input.inp_Turbo && !m_inputFreeze 
            && m_magicCurrent > m_magicAmtTurbo)
        {
            m_moveSpeed += m_speedTurbo;
            m_magicCurrent -= m_magicAmtTurbo;
        }
    }

    private void Falling()
    {
        if (m_grounded == false && m_ramping == false)
        {
            m_gravity += Physics.gravity.y*m_gravityMultiplier;
            if (m_gravity < m_gravityMax)
                m_gravity = m_gravityMax;
        }
    }

    private void ValidateSpeed()
    {
        if (m_moveSpeed > m_maxSpeedTotal)
            m_moveSpeed = m_maxSpeedTotal;
        else if (m_moveSpeed < 0)
            m_moveSpeed = 0;
    }

    private void MoveToCurrentLane()
    {
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

    private void GroundAnimationLogic()
    {
        if (m_animator.GetBool("grounded") == false)
            m_animator.SetBool("grounded", true);

        if (m_body.velocity.x < 0.1f)
        {
            m_animator.ResetTrigger("skate");
            m_animator.SetTrigger("idle");
        }
        else if (m_animationChangeReady)
        {
            m_animator.ResetTrigger("idle");
            m_animator.SetTrigger("skate");
            var animationSpeed = ((5 + m_maxSpeedTotal) - m_moveSpeed)/5;
            //Debug.Log("animation speed=" + animationSpeed);
            m_animator.speed = animationSpeed;
            m_animationChangeReady = false;
        }
        else
        {
            if (m_animationChangeTimer > m_animationChangeTime)
            {
                m_animationChangeTimer = 0f;
                m_animationChangeReady = true;
            }
            else
            {
                m_animationChangeTimer += Time.fixedDeltaTime;
            }
        }
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

    private void CheckRaycastDirections()
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
                if (m_body.velocity.x < 0.1f)
                    m_gravity += m_jump/10;
                //Debug.Log("x=" + m_body.velocity.x);
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
            //Debug.Log("Deteceted a thing on the next lane up");
            m_canShiftUp = false;
        }
        else
        {
            m_canShiftUp = true;
        }

        Debug.DrawRay(checkFrom, -checkTo, Color.cyan);
        if (Physics.Raycast(checkFrom, -checkTo, out hit, 1, mask))
        {
            //Debug.Log("Deteceted a thing on the next lane down");
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
            //Debug.Log("collider hit " + hit.otherCollider.gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var mask = (LayerMask.NameToLayer("Obstacle"));
        if (other.gameObject.layer == mask)
        {
            HitObstacle();
        }

    }

    public void HitObstacle()
    {
        //Debug.Log("lose mega-time");
        m_body.mass = 6f;
        m_moveSpeed = 0f;
        if (m_grounded)
            m_body.AddForce(transform.up * m_jump);
        StartCoroutine("FlashRenderer");
        FreezeInput(m_obstacleFreezeInputTime);
        m_body.mass = 2f;
    }

    public IEnumerator FlashRenderer()
    {
        m_renderer.enabled = false;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 6);
        m_renderer.enabled = true;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 6);
        m_renderer.enabled = false;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 6);
        m_renderer.enabled = true;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 6);
        m_renderer.enabled = false;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 6);
        m_renderer.enabled = true;
    }

    public void FreezeInput(float seconds)
    {
        m_inputFreeze = true;
        m_inputFreezeTime = seconds;
    }

    private void CheckForLaneChange()
    {
        var ChangedLane = true;
        if (SystemsManager.m_Input.inp_D_Up && m_canShiftUp && !m_inputFreeze)
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
        else if (SystemsManager.m_Input.inp_D_Down && m_canShiftDown && !m_inputFreeze)
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

            m_moveSpeed *= m_laneChangePenaltyMultiplier;
        }
    }

    private void CheckForRampMagic()
    {
        if (SystemsManager.m_Input.inp_Ramp && m_magicCurrent > m_magicAmtRamp)
        {
            StartCoroutine("RampMagic", (int)m_laneCurrent);
        }
    }

    private IEnumerator RampMagic(int lane)
    {
        Debug.Log("placing ramp on lane " + lane+1);

        m_magicCurrent -= m_magicAmtRamp;

        SystemsManager.m_Game.m_currentLevel.PlaceRampOnLane(lane, m_tilesPassed);

        yield return null;
    }

    private void CheckForTrick()
    {
        if (m_tricking == false && m_grounded == false && m_ramping == false)
        {
            if (SystemsManager.m_Input.inp_Trick)
            {
                StartCoroutine("Trick");
            }
        }
        else
        {
            m_trickTimer += Time.fixedDeltaTime;

            if (m_trickTimer > m_trickTime)
            {
                m_trickTimer = 0f;
                m_tricking = false;
            }
        }
    }

    private IEnumerator Trick()
    {
        m_animator.ResetTrigger("idle");
        m_animator.ResetTrigger("jump");
        m_animator.ResetTrigger("skate");
        m_animator.SetTrigger("trick");
        m_animator.SetBool("tricking", true);

        m_tricking = true;

        SystemsManager.m_Score.ScoreTrick();

        m_magicCurrent += m_magicAmtTrick;

        //SystemsManager.m_SoundFX.Trick();

        yield return new WaitForSeconds(0.5f);

        m_animator.ResetTrigger("trick");
        m_animator.SetBool("tricking", false);
    }

    private void CheckForJump()
    {
        if (m_jumpReady)
        {
            if (SystemsManager.m_Input.inp_Jump && !m_inputFreeze)
            {
                StartCoroutine("Jump");
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

    private IEnumerator Jump()
    {
        m_animator.SetBool("jumping", true);
        m_animator.ResetTrigger("idle");
        //yield return new WaitForSeconds(0.01f);
        m_animator.ResetTrigger("skate");
        //yield return new WaitForSeconds(0.01f);
        m_animator.SetTrigger("jump");

        m_gravity += m_jump;

        Vector3 tempVect = transform.position;
        tempVect.y -= m_groundCheckLength;
        SystemsManager.m_Particles.Fire_Olli(tempVect);

        SystemsManager.m_SoundFX.OneShot_Jump();

        m_jumpReady = false;

        yield return new WaitForSeconds(0.75f);

        m_animator.ResetTrigger("jump");
        m_animator.SetBool("jumping", false);
    }
}