using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrolleyPlayerController : MonoBehaviour
{
    public enum TrolleyMovementState
    {
        Railed,
        Derailed,
        Free,
        End
    }


    public TrolleyMovementState CurrentMovementState { get; private set; } = TrolleyMovementState.Free;

    public float speed = 0.000f;
    public float acceleration { get; private set; } = 0.001f;
    public float maxSpeed { get; private set; } = 0.6f;
    public float minSpeed { get; private set; } = -0.3f;

    private SparkController sparkController;

    public Transform parentTransform { get; private set; } = null;
    public Rigidbody parentRigibody = null;
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] public GameObject cameraFree = null;
    private Vector3 camRelPosition = Vector3.zero;
    private Vector3 camRelRotation = Vector3.zero;
    private Vector3 freeForward = Vector3.zero;

    public float lapBoost { get; private set; } = 1;

    public float baseImpulse;

    private bool turning = false;

    private bool turningRight = true;

    public void SetLapBoost(int boost)
    {
        lapBoost = boost * .1f - .2f;
    }

    public void ResetRotation()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    public void SetMovementState(TrolleyMovementState state)
    {
        CurrentMovementState = state;

        switch (CurrentMovementState)
        {
            case TrolleyMovementState.Railed:
                parentRigibody.velocity = Vector3.zero;
                break;
            case TrolleyMovementState.Free:
                DisableSparks();
                break;
        }
    }

    public void SetSparks(bool right, bool enabled = true)
    {
        if (right)
        {
            sparkController.EnableRightSparks(enabled);
        }
        else
        {
            sparkController.EnableLeftSparks(enabled);
        }
    }

    public void SetInitialForward(Vector3 forward)
    {
        tmsRailed.SetInitialForward(forward);
    }

    public void SetCameraPos(Transform pos)
    {
        mainCamera.transform.SetParent(transform.parent);
        mainCamera.transform.position = pos.position;
        mainCamera.transform.rotation = pos.rotation;
    }

    public void ResetCamera()
    {
        mainCamera.transform.SetParent(transform);
        mainCamera.transform.localPosition = camRelPosition;
        mainCamera.transform.localEulerAngles = camRelRotation;
        ResetRotation();
    }


    [SerializeField] private TrolleyMoveState_Railed tmsRailed;
    // Start is called before the first frame update
    void Start()
    {
        SetInitialForward(transform.forward);
        freeForward = transform.forward;

        parentTransform = transform.parent;
        parentRigibody = parentTransform.GetComponent<Rigidbody>();

        sparkController = GetComponent<SparkController>();

        mainCamera.transform.SetParent(transform);
        camRelPosition = mainCamera.transform.localPosition;
        camRelRotation = mainCamera.transform.localEulerAngles;
    }


    // Update is called once per frame
    void Update()
    {
        if (speed > maxSpeed)
        {
            speed -= .005f;
        }
        switch (CurrentMovementState)
        {
            case TrolleyMovementState.Railed:
                tmsRailed.UpdateRailed();
                break;
            case TrolleyMovementState.Derailed:
                UpdateDerailed();
                break;
            case TrolleyMovementState.Free:
                UpdateFree();
                break;
        }
    }

    Vector3 derailedVector;
    public float derailedDeceleration = 0.0003f;


    public void Decelerate()
    {
        if (speed > minSpeed)
        {
            speed -= acceleration;
        }
    }


    public void SetDerailedVector(Vector3  derailed)
    {
        derailedVector = derailed;
    }

    void UpdateDerailed()
    {
        transform.forward = derailedVector;
        speed -= derailedDeceleration;
        if (speed < 0)
            speed = 0;
        float tempLapBoost = lapBoost;
        if (speed <= Mathf.Abs(lapBoost))
            tempLapBoost = 0;
        parentRigibody.velocity = (speed + tempLapBoost) * DerailedSpeedMultiplier * tmsRailed.RailInitialForward;

        // add vibrations to feel like you are vibrating

        // add sparks

        if (Input.GetKeyDown("space"))
        {
            SetMovementState(TrolleyMovementState.Railed);
            DisableSparks();
        }


    }

    private void DisableSparks()
    {
        SetSparks(true, false);
        SetSparks(false, false);
    }

    private float DerailedSpeedMultiplier = 200;

    void UpdateFree()
    {
        if (Input.GetKey("j"))
        {
            SetMovementState(TrolleyMovementState.Railed);
        }



        if (Input.GetKey("up"))
        {
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
        }

        if (Input.GetKey("down"))
        {
            Decelerate();
        }

        if (Input.GetKey("right"))
        {
            parentTransform.RotateAround(transform.position, Vector3.up, 1f);
            float angle = Vector3.SignedAngle(freeForward, transform.forward, Vector3.up);
            if (!turning && Mathf.Abs(angle) >= 45)
            {
                int dir = 0;
                if (angle <= -45)
                    dir = 1;
                else
                    dir = -1;
                parentTransform.RotateAround(transform.position, Vector3.up, dir);
            }
        }
        else if (Input.GetKey("left"))
        {
            parentTransform.RotateAround(transform.position, Vector3.up, -1f);
            float angle = Vector3.SignedAngle(freeForward, transform.forward, Vector3.up);
            if (!turning && Mathf.Abs(angle) >= 45)
            {
                int dir = 0;
                if (angle <= -45)
                    dir = 1;
                else
                    dir = -1;
                parentTransform.RotateAround(transform.position, Vector3.up, dir);
            }
        }
        else if (!turning)
        {
            int dir = 0;
            float angle = Vector3.SignedAngle(freeForward, transform.forward, Vector3.up);
            if (transform.forward != freeForward)
            {
                if (angle < .5f && angle > -.5f)
                    transform.forward = freeForward;
                else
                {
                    if (angle < 0)
                        dir = 1;
                    else
                        dir = -1;
                    parentTransform.RotateAround(transform.position, Vector3.up, dir * .25f);
                }
            }
        }
        if (turning)
        {
            float angle = Vector3.SignedAngle(freeForward, transform.forward, Vector3.up);
            float minAngle = 0;
            float maxAngle = 0;
            if (turningRight)
                maxAngle = 90;
            else
                minAngle = -90;
            if (angle > maxAngle || angle < minAngle)
            {
                int dir = 0;
                if (angle < minAngle)
                    dir = 1;
                else
                    dir = -1;
                parentTransform.RotateAround(transform.position, Vector3.up, dir);
            }
        }
        Vector3 forward = parentTransform.forward;


        forward.y = 0f;
        float tempLapBoost = lapBoost;
        if (speed <= Mathf.Abs(lapBoost))
            tempLapBoost = 0;

        if (Time.time > boostTime + boostDuration)
        {
            boostMultiplier = 1f;
        }
        parentRigibody.velocity = ((speed + tempLapBoost) * freeSpeedMultiplier) * boostMultiplier * forward;

    }

    public float freeSpeedMultiplier = 400f;

    public void AddBoost(float morality)
    {
        boostTime = Time.time;
        boostDuration = morality / 2f;
        boostMultiplier = 2f;
        //parentRigibody.AddForce(transform.forward * baseImpulse * morality);
        //speed = parentRigibody.velocity.magnitude / freeSpeedMultiplier;
    }

    public float boostDuration;  
    public float boostMultiplier = 2f;
    public float boostTime = 0f;


    public void SetTurning(bool right)
    {
        turning = true;
        turningRight = right;
    }

    public void StopTurning(Vector3 newForward)
    {
        turning = false;
        freeForward = newForward;
    }
}
