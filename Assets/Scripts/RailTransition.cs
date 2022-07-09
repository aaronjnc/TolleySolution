using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTransition : MonoBehaviour
{
    [SerializeField]
    private bool railStart;

    private void OnTriggerEnter(Collider other)
    {
        TrolleyPlayerController tp = other.GetComponent<TrolleyPlayerController>();
        if (tp != null)
        {
            if (railStart)
            {
                tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Railed);
            }
            else
            {
                tp.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Free);
            }
        }
    }
}
