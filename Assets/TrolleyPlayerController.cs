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

    public float lapBoost { get; private set; } = 1;

    public float baseImpulse;

    public void SetLapBoost(int boost)
    {
        lapBoost = boost * .1f;
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
        parentRigibody.velocity = (speed + lapBoost) * DerailedSpeedMultiplier * tmsRailed.RailInitialForward;

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

        }
        else if (Input.GetKey("left"))
        {
            parentTransform.RotateAround(transform.position, Vector3.up, -1f);
        }

        Vector3 forward = parentTransform.forward;


        forward.y = 0f;

        parentRigibody.velocity = ((speed + lapBoost) * freeSpeedMultiplier) * forward;

    }

    public float freeSpeedMultiplier = 400f;

    public void AddBoost(float morality)
    {
        parentRigibody.AddForce(transform.forward * baseImpulse * morality);
        speed = parentRigibody.velocity.magnitude / freeSpeedMultiplier;
    }

}
