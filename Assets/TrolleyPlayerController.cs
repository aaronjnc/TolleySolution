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


    public TrolleyMovementState CurrentMovementState { get; private set; } = TrolleyMovementState.Free;

    public float speed = 0.000f;
    public float acceleration { get; private set; } = 0.001f;
    public float maxSpeed { get; private set; } = 0.6f;
    public float minSpeed { get; private set; } = -0.3f;
    public bool boosted { get; private set; }  = false;
    public float maxBoostedSpeed { get; private set; }

        

    //public Vector3 RailInitialForward;
    //Vector3 RailInitialRight;
    public Transform parentTransform { get; private set; }  = null;
    Rigidbody parentRigibody = null;
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] public GameObject cameraFree = null;
    private Vector3 camRelPosition = Vector3.zero;
    private Vector3 camRelRotation = Vector3.zero;

    public void ResetRotation()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    public void SetMovementState(TrolleyMovementState state)
    {
        CurrentMovementState = state;
        if (CurrentMovementState == TrolleyMovementState.Free)
        {
        }
        else if (CurrentMovementState == TrolleyMovementState.Railed)
        {
            parentRigibody.velocity = Vector3.zero;
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

        mainCamera.transform.SetParent(transform);
        camRelPosition = mainCamera.transform.localPosition;
        camRelRotation = mainCamera.transform.localEulerAngles;
    }


    // Update is called once per frame
    void Update()
    {
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


    public void Declerate()
    {
        if (!boosted)
        {
            if (speed > minSpeed)
            {
                speed -= acceleration;
            }
        }
        else
        {
            speed = Mathf.Clamp(speed - acceleration, minSpeed, maxBoostedSpeed);
            maxBoostedSpeed = speed;
            if (speed <= maxSpeed)
                boosted = false;
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
        parentTransform.position += speed * tmsRailed.RailInitialForward;

        // add vibrations to feel like you are vibrating

        // add sparks

        if (Input.GetKeyDown("space"))
        {
            SetMovementState(TrolleyMovementState.Railed);
        }


    }

    void UpdateFree()
    {
        if (Input.GetKey("j"))
        {
            SetMovementState(TrolleyMovementState.Railed);
        }



        if (Input.GetKey("up"))
        {
            if (!boosted)
            {
                if (speed < maxSpeed)
                {
                    speed += acceleration;
                }
            }
        }

        if (Input.GetKey("down"))
        {
            Declerate();
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

        parentRigibody.velocity = (speed * freeSpeedMultiplier) * forward;

    }

    public float freeSpeedMultiplier = 400f;

    public void AddBoost(float morality)
    {
        maxBoostedSpeed = .5f;
        speed = .3f + morality * .06f;
        boosted = true;
    }

}
