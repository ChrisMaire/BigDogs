using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public float inp_DeadZone = 0.4f;
    public bool inp_D_Down;
    public bool inp_D_Left;
    public bool inp_D_Right;
    public bool inp_D_Up;

    public bool inp_Pause;
    public bool inp_Jump;
    public bool inp_Trick;
    public bool inp_Turbo;
    public bool inp_Ramp;
    public bool inp_Skate;
    
    void Update()
    {
        InputCheck();
    }

    void InputCheck()
    {
        if (Input.GetAxis("Vertical") < -inp_DeadZone || Input.GetAxis("DPad-Vert") < -inp_DeadZone)
            inp_D_Up = true;
        else
        {
            if (Input.GetButton("Keyb-Up"))
                inp_D_Up = true;
            else
                inp_D_Up = false;
        }

        if (Input.GetAxis("Vertical") > inp_DeadZone || Input.GetAxis("DPad-Vert") > inp_DeadZone)
            inp_D_Down = true;
        else
        {
            if (Input.GetButton("Keyb-Down"))
                inp_D_Down = true;
            else
                inp_D_Down = false;
        }

        if (Input.GetAxis("Horizontal") < -inp_DeadZone || Input.GetAxis("DPad-Horiz") < -inp_DeadZone)
            inp_D_Right = true;
        else
        {
            if (Input.GetButton("Keyb-Right"))
                inp_D_Right = true;
            else
                inp_D_Right = false;
        }

        if (Input.GetAxis("Horizontal") > inp_DeadZone || Input.GetAxis("DPad-Horiz") > inp_DeadZone)
            inp_D_Left = true;
        else
        {
            if (Input.GetButton("Keyb-Left"))
                inp_D_Left = true;
            else
                inp_D_Left = false;
        }

        if (Input.GetButton("Jump") || Input.GetButton("Jump2"))
            inp_Jump = true;
        else
            inp_Jump = false;
        if (Input.GetButton("Push") || Input.GetButton("Push2"))
            inp_Skate = true;
        else
            inp_Skate = false;

        if (Input.GetButtonDown("Trick") || Input.GetButton("Trick2"))
            inp_Trick = true;
        else
            inp_Trick = false;

        if (Input.GetAxis("Turbo3") > inp_DeadZone || Input.GetAxis("Turbo3") < -inp_DeadZone)
            inp_Turbo = true;
        else
        {
            if (Input.GetButtonDown("Turbo") || Input.GetButton("Turbo2"))
                inp_Turbo = true;
            else
                inp_Turbo = false;
        }

        if (Input.GetButtonDown("Ramp") || Input.GetButton("Ramp2"))
            inp_Ramp = true;
        else
            inp_Ramp = false;

        if (Input.GetButtonDown("Pause"))
            inp_Pause = true;
        else
            inp_Pause = false;
    }
}