using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public float inp_DeadZone = 0.4f;
    public bool AxisInUse_1X;
    public bool AxisInUse_1Y;
    public bool AxisInUse_2X;
    public bool AxisInUse_2Y;
    public bool AxisInUse_TriggerL;
    public bool AxisInUse_TriggerR;
    public bool inp_D_Down;
    public bool inp_D_Left;
    public bool inp_D_Right;
    public bool inp_D_Up;

    public bool inp_Pause;
    public bool inp_Jump;
    public bool inp_Trick1;
    public bool inp_Trick2;
    public bool inp_Trick3;
    public bool inp_Push;
    
    void Update()
    {
        InputCheck(); ;
    }

    void InputCheck()
    {
        if (Input.GetAxis("Vertical") > inp_DeadZone || Input.GetAxis("DPad-Vert") > inp_DeadZone)
            AxisInUse_1Y = true;
        else
        {
            if (Input.GetButton("Keyb-Up"))
                inp_D_Up = true;
            else
                inp_D_Up = false;
        }
        if (Input.GetAxis("Vertical") < -inp_DeadZone || Input.GetAxis("DPad-Vert") < -inp_DeadZone)
            AxisInUse_1Y = true;
        else
        {
            if (Input.GetButton("Keyb-Down"))
                inp_D_Down = true;
            else
                inp_D_Down = false;
        }

        if (Input.GetAxis("Horizontal") > inp_DeadZone || Input.GetAxis("DPad-Horiz") > inp_DeadZone)
        {
            AxisInUse_1X = true;

        }
        else
        {
            if (Input.GetButton("Keyb-Right"))
                inp_D_Right = true;
            else
                inp_D_Right = false;
        }
        if (Input.GetAxis("Horizontal") < -inp_DeadZone || Input.GetAxis("DPad-Horiz") < -inp_DeadZone)
            AxisInUse_1X = true;
        else
        {
            if (Input.GetButton("Keyb-Left"))
                inp_D_Left = true;
            else
                inp_D_Left = false;
        }

        if (Input.GetButtonDown("Olli"))
            inp_Jump = true;
        else
            inp_Jump = false;
        if (Input.GetButtonDown("Push") || Input.GetButtonDown("Push2"))
            inp_Push = true;
        else
            inp_Push = false;

        if (Input.GetButtonDown("Trick1"))
            inp_Trick1 = true;
        else
            inp_Trick1 = false;
        if (Input.GetButtonDown("Trick2"))
            inp_Trick2 = true;
        else
            inp_Trick2 = false;
        if (Input.GetButtonDown("Trick3"))
            inp_Trick3 = true;
        else
            inp_Trick3 = false;

        if (Input.GetButtonDown("Pause"))
            inp_Pause = true;
        else
            inp_Pause = false;
    }
}
