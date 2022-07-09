using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyPlayerController : MonoBehaviour
{
    //Vector3 speed = new Vector3(0.1f, 0, 0);
    //Vector3 acceleration = new Vector3 (0.1f, 0, 0);


    public enum TrolleyMovementState
    {
        Railed,
        Derailed,
        Free,
        End
    }

    public TrolleyMovementState CurrentMovementState { get; private set; } = TrolleyMovementState.Railed;

    public float speed = 0.000f;
    float acceleration = 0.001f;
    float maxSpeed = 0.3f;
    float minSpeed = -0.3f;

    float angle = 0f;
    float angleSpeed = 1f;
    float maxAngle = 25f;
    float minAngle = -25f;

    Vector3 InitialForward;
    Vector3 Initialright;
    Transform parentTransform = null;


    // Start is called before the first frame update
    void Start()
    {
        InitialForward = transform.forward;
        Initialright = Vector3.Cross(Vector3.up, InitialForward);
        parentTransform = transform.parent;
        downTime[0] = -1f;
        downTime[1] = -1f;
    }


    // Update is called once per frame
    void Update()
    {

        //transform.position += speed;
        switch (CurrentMovementState)
        {
            case TrolleyMovementState.Railed:
                UpdateRailed();
                break;
            case TrolleyMovementState.Derailed:
                UpdateDerailed();
                break;
        }
    }


    Vector3 derailedVector;
    float derailedDeceleration = 0.003f;

    void UpdateDerailed()
    {
        transform.forward = derailedVector;
        speed -= derailedDeceleration;
        if (speed < 0)
            speed = 0;
        parentTransform.position += speed * InitialForward;

        // add vibrations to feel like you are vibrating

        // add sparks

        if (Input.GetKeyDown("space"))
        {
            CurrentMovementState = TrolleyMovementState.Railed;
        }


    }

    

    void UpdateRailed()
    {

        ReadInputsRailed();
        

        if (angle > 0)
        {
            transform.forward = Vector3.Slerp(InitialForward, Initialright, angle / 90f);
        }
        else
        {
            transform.forward = Vector3.Slerp(InitialForward, -Initialright, angle / -90f);
        }

        parentTransform.position += speed * InitialForward;
    }


    private void ReadInputsRailed()
    {
        if (Input.GetKey("up"))
        {
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
        }

        if (Input.GetKey("down"))
        {
            if (speed > minSpeed)
            {
                speed -= acceleration;
            }
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
                CurrentMovementState = TrolleyMovementState.Derailed;
                derailedVector = (angle > 0f) ? Initialright : -Initialright;
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
                CurrentMovementState = TrolleyMovementState.Derailed;
                derailedVector = (angle > 0f) ? Initialright : -Initialright;
            }
        }
        else
        {
            // TODO gradually transition angle to 0f;
            angle = 0f;
        }


        checkSwitchTracks(Input.GetKeyDown("left"), ref downTime[0]);
        checkSwitchTracks(Input.GetKeyDown("right"), ref downTime[1]);
    }


    private bool checkSwitchTracks(bool keyDown, ref float downTime)
    {
        if (keyDown)
        {
            if (downTime > 0)
            {
                float deltaTime = Time.time - downTime;
                if (deltaTime < doublePressTime)
                {
                    switchTracks();
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

        }
        print("you are trying to switch tracks");
    }

}
