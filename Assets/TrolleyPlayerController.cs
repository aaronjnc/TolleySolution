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



        if (Input.GetKeyDown("space"))
        {
            // enter Rail
            print("up arrow key is held down");
            CurrentMovementState = TrolleyMovementState.Railed;
            //derailedVector = (angle > 0f) ? Initialright : -Initialright;
        }


    }

    void UpdateRailed()
    {

        if (Input.GetKey("up"))
        {
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
            //print("up arrow key is held down");
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
                print("up arrow key is held down");
                CurrentMovementState = TrolleyMovementState.Derailed;
                derailedVector = (angle > 0f) ? Initialright : -Initialright;
            }

        }
        else if (Input.GetKey("left"))
        {
            if (angle > minAngle)
            {
                angle -= angleSpeed;
            }else if (Input.GetKeyDown("space"))
                {
                    // enterDerailment
                    print("up arrow key is held down");
                    CurrentMovementState = TrolleyMovementState.Derailed;
                    derailedVector = (angle > 0f) ? Initialright : -Initialright;
                }
        }
        else
        {
            // TODO gradually transition angle to 0f;
            angle = 0f;
        }




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


}
