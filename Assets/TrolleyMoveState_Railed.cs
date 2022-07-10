using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyMoveState_Railed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        tp = GetComponent<TrolleyPlayerController>();
        downTime[0] = -1f;
        downTime[1] = -1f;
    }

    TrolleyPlayerController tp = null;
    public Vector3 RailInitialForward;
    Vector3 RailInitialRight;
    float angle = 0f;

    float angleSpeed = 1f;
    float maxAngle = 25f;
    float minAngle = -25f;

    Rail currentRail = null;

    public void SetInitialForward(Vector3 forward)
    {
        RailInitialForward = forward;
        RailInitialRight = Vector3.Cross(Vector3.up, RailInitialForward);
    }



    public void UpdateRailed()
    {
        ReadInputsRailed();

        if (tp.CurrentMovementState == TrolleyPlayerController.TrolleyMovementState.Free)
        {
            transform.forward = RailInitialForward;
        }
        else
        {
            if (angle > 0)
            {
                transform.forward = Vector3.Slerp(RailInitialForward, RailInitialRight, angle / 90f);
            }
            else
            {
                transform.forward = Vector3.Slerp(RailInitialForward, -RailInitialRight, angle / -90f);
            }
        }

        tp.parentRigibody.velocity = (tp.speed + tp.lapBoost) * RailedSpeedMultiplier * RailInitialForward;
    }

    private float RailedSpeedMultiplier = 400;


    private void ReadInputsRailed()
    {
        if (Input.GetKey("up"))
        {
            if (tp.speed < tp.maxSpeed)
            {
                tp.speed += tp.acceleration;
            }
        }

        if (Input.GetKey("down"))
        {
            tp.Decelerate();
        }

        if (Input.GetKey("right"))
        {
            if (angle < maxAngle)
            {
                angle += angleSpeed;
            }
            else if (Input.GetKeyDown("space"))
            {
                // enterDerailment
                tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Derailed);
                tp.SetDerailedVector((angle > 0f) ? RailInitialRight : -RailInitialRight);
                tp.SetSparks(true);
            }

        }
        else if (Input.GetKey("left"))
        {
            if (angle > minAngle)
            {
                angle -= angleSpeed;
            }
            else if (Input.GetKeyDown("space"))
            {
                // enterDerailment
                tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Derailed);
                tp.SetDerailedVector((angle > 0f) ? RailInitialRight : -RailInitialRight);
                tp.SetSparks(false);
            }
        }
        else
        {
            // TODO gradually transition angle to 0f;
            angle = 0f;
        }


        if (Input.GetKey("f"))
        {
            tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Free);
        }


        checkSwitchTracks(Input.GetKeyDown("left"), ref downTime[0], false);
        checkSwitchTracks(Input.GetKeyDown("right"), ref downTime[1], true);
    }


    private bool checkSwitchTracks(bool keyDown, ref float downTime, bool isRight)
    {
        if (keyDown)
        {
            if (downTime > 0)
            {
                float deltaTime = Time.time - downTime;
                if (deltaTime < doublePressTime)
                {
                    switchTracks(isRight);
                }
                else
                {
                    downTime = Time.time;
                }
            }
            else
            {
                downTime = Time.time;
            }
        }
        return false;
    }


    private readonly float[] downTime = new float[2];
    private float doublePressTime = 0.75f;

    private void switchTracks(bool right)
    {
        if (right)
        {
            currentRail.SwitchLane(1);
        }
        else
        {
            currentRail.SwitchLane(-1);
        }
    }

    public void SetRail(Rail rail)
    {
        currentRail = rail;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
