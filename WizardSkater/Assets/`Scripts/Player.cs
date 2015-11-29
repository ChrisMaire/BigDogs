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

    [Header("Movement")] public float m_gravityGround = -0.7f;
    private float m_gravity;

    [Header("Collision")] private bool m_grounded;
    public bool m_hitObject;

    [Header("Jump")] public float m_jump = 8.0F;
    [Range(0.15f, 2.0f)] public float m_jumpAgainTime = 0.5f;
    private float m_jumpAgainTimer;
    private bool m_jumpReady;

    [Header("Speed")] public float m_speedAccel = 0.5F;
    public float m_speedMax = 10.0F;
    public float m_speedLossLangeChange = 10.0F;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();

    }
}