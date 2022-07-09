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
        public float maxSpeed { get; private set; } = 0.3f;
        public float minSpeed { get; private set; } = -0.3f;
        public bool boosted { get; private set; }  = false;
        public float maxBoostedSpeed { get; private set; }

        

        //public Vector3 RailInitialForward;
        //Vector3 RailInitialRight;
        public Transform parentTransform { get; private set; }  = null;
        Rigidbody parentRigibody = null;
        [SerializeField] private Camera mainCamera = null;
        [SerializeField] public GameObject cameraFree = null;
        private Quaternion railedCameraRotation;
        private Vector3 railedCameraPosition;

        public void ResetRotation()
        {
            transform.localEulerAngles = Vector3.zero;
        }

        public void SetMovementState(TrolleyMovementState state)
        {
            CurrentMovementState = state;
            if (CurrentMovementState == TrolleyMovementState.Free)
            {
                //Camera.main.transform.SetParent(transform);
            }
            else if (CurrentMovementState == TrolleyMovementState.Railed)
            {
                parentRigibody.velocity = Vector3.zero;
                /*Camera.main.transform.SetParent(transform);
                mainCamera.transform.localPosition = railedCameraPosition;
                mainCamera.transform.localRotation = railedCameraRotation;
                Camera.main.transform.SetParent(transform.parent);*/
            }
        }

        public void SetInitialForward(Vector3 forward)
        {
            tmsRailed.SetInitialForward(forward);
        }

        public void SetCameraPos(Transform pos)
        {
            mainCamera.transform.position = pos.position;
            mainCamera.transform.rotation = pos.rotation;
        }

        public void ResetCamera()
        {
            mainCamera.transform.position = cameraFree.transform.position;
            mainCamera.transform.forward = cameraFree.transform.forward;
            ResetRotation();
        }

        Vector3 FreeCameraOffset;

        [SerializeField] private TrolleyMoveState_Railed tmsRailed;
        // Start is called before the first frame update
        void Start()
        {
            SetInitialForward(transform.forward);



            parentTransform = transform.parent;
            parentRigibody = parentTransform.GetComponent<Rigidbody>();


            FreeCameraOffset = mainCamera.transform.localPosition - transform.localPosition;
            //downTime[0] = -1f;
            //downTime[1] = -1f;
            //mainCamera.transform.SetParent(transform);
            //railedCameraRotation = mainCamera.transform.localRotation;
            //railedCameraPosition = mainCamera.transform.localPosition;
            //mainCamera.transform.SetParent(transform.parent);
        }


        // Update is called once per frame
        void Update()
        {

            //transform.position += speed;
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
        float derailedDeceleration = 0.003f;


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
            // move camera
            //mainCamera.transform.position = cameraFree.transform.position;
            //mainCamera.transform.forward = cameraFree.transform.forward;
            //ResetRotation();

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
                if (!boosted)
                {
                    if (speed > minSpeed)
                    {
                        speed -= acceleration;
                    }
                }
                else
                {
                    speed = Mathf.Clamp(speed + acceleration, minSpeed, maxBoostedSpeed);
                    maxBoostedSpeed = speed;
                    if (speed <= maxSpeed)
                        boosted = false;
                }
            }

            if (Input.GetKey("right"))
            {
                parentTransform.Rotate(0f, 1f, 0f);

            }
            else if (Input.GetKey("left"))
            {
                parentTransform.Rotate(0f, -1f, 0f);
            }

            Vector3 forward = parentTransform.forward;


            forward.y = 0f;

            parentRigibody.velocity = (speed * 1000) * forward;

        }

        /*
        void UpdateRailed()
        {

            //mainCamera.transform.localPosition = railedCameraPosition;
            //mainCamera.transform.localRotation = railedCameraRotation;

            ReadInputsRailed();

            if (CurrentMovementState == TrolleyMovementState.Free)
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

            parentTransform.position += speed * RailInitialForward;
        }*/

        /*
        private void ReadInputsRailed()
        {
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

            if (Input.GetKey("right"))
            {
                if (angle < maxAngle)
                {
                    angle += angleSpeed;
                }
                else if (Input.GetKeyDown("space"))
                {
                    // enterDerailment
                    SetMovementState(TrolleyMovementState.Derailed);
                    derailedVector = (angle > 0f) ? RailInitialRight : -RailInitialRight;
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
                    SetMovementState(TrolleyMovementState.Derailed);
                    derailedVector = (angle > 0f) ? RailInitialRight : -RailInitialRight;
                }
            }
            else
            {
                // TODO gradually transition angle to 0f;
                angle = 0f;
            }


            if (Input.GetKey("f"))
            {
                SetMovementState(TrolleyMovementState.Free);
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
                //print("you are trying to switch tracks right ");
                transform.position = transform.position + 10 * RailInitialRight;
                cameraFree.transform.position = cameraFree.transform.position + 10 * RailInitialRight;
                //transform.position = new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = transform.position - 10 * RailInitialRight;
                cameraFree.transform.position = cameraFree.transform.position - 10 * RailInitialRight;
                //transform.position = new Vector3(transform.position.x - 10f, transform.position.y, transform.position.z);
            }
        }
        */

        public void AddBoost(float morality)
        {
            maxBoostedSpeed = .5f;
            speed = .3f + morality * .06f;
            boosted = true;
        }

    }
