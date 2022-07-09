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

        public void SetInitialForward(Vector3 forward)
        {
            RailInitialForward = forward;
            RailInitialRight = Vector3.Cross(Vector3.up, RailInitialForward);
        }



        public void UpdateRailed()
        {

            //mainCamera.transform.localPosition = railedCameraPosition;
            //mainCamera.transform.localRotation = railedCameraRotation;

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

            tp.parentTransform.position += tp.speed * RailInitialForward;
        }


        private void ReadInputsRailed()
        {
            if (Input.GetKey("up"))
            {
                if (!tp.boosted)
                {
                    if (tp.speed < tp.maxSpeed)
                    {
                        tp.speed += tp.acceleration;
                    }
                }
            }

            if (Input.GetKey("down"))
            {
                tp.Declerate();
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
                //print("you are trying to switch tracks right ");
                transform.position = transform.position + 10 * RailInitialRight;
                tp.cameraFree.transform.position = tp.cameraFree.transform.position + 10 * RailInitialRight;
                //transform.position = new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = transform.position - 10 * RailInitialRight;
                tp.cameraFree.transform.position = tp.cameraFree.transform.position - 10 * RailInitialRight;
                //transform.position = new Vector3(transform.position.x - 10f, transform.position.y, transform.position.z);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
