using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTransition : MonoBehaviour
{
    [SerializeField]
    private bool railStart;

    [SerializeField]
    private Rail rail;

    [SerializeField]
    private Transform cameraPos;

    void Start()
    {

        rail = transform.parent.GetComponent<Rail>();
    }

    private void OnTriggerEnter(Collider other)
    {
        TrolleyPlayerController tp = other.GetComponent<TrolleyPlayerController>();
        if (tp != null)
        {
            if (railStart)
            {
                tp.SetInitialForward(transform.forward);
                tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Railed);
                tp.SetCameraPos(cameraPos);
            }
            else
            {
                tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Free);
                tp.ResetCamera();
            }
        }
    }
}
