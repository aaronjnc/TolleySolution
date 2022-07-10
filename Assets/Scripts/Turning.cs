using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turning : MonoBehaviour
{
    [SerializeField]
    private bool turningRight = false;

    [SerializeField]
    private Vector3 newForward = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        TrolleyPlayerController controller = other.GetComponent<TrolleyPlayerController>();
        if (controller != null)
        {
            controller.SetTurning(turningRight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TrolleyPlayerController controller = other.GetComponent<TrolleyPlayerController>();
        if (controller != null)
        {
            controller.StopTurning(newForward);
        }
    }
}
