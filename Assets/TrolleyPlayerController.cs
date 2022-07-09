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
    float acceleration = 0.01f;
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
    float derailedDeceleration = 0.3f;

    void UpdateDerailed()
    {
        transform.forward = InitialForward;
        speed -= derailedDeceleration;
        if (speed < 0)
            speed = 0;
        parentTransform.position += speed * InitialForward;



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

        }else
        if (Input.GetKey("left"))
        {
            if (angle > minAngle)
            {
                angle -= angleSpeed;
            }
        }
        else
        {
            angle = 0f;
        }



        if (Input.GetKey("space"))
        {
            if (Mathf.Abs(angle) > maxAngle)
            {
                // enterDerailment
                print("up arrow key is held down");
                CurrentMovementState = TrolleyMovementState.Derailed;
                derailedVector = (angle > 0f) ? Initialright : -Initialright;
            }
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
