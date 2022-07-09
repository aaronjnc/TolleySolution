using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTransition : MonoBehaviour
{
    [SerializeField]
    private bool railStart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Trolley"))
        {
            TrolleyPlayerController tp = other.GetComponent<TrolleyPlayerController>();
            if (tp != null)
            {
                
            }
        }
    }
}
