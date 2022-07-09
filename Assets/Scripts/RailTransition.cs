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
                rail.StartRail();
            }
            else
            {
                rail.StopRail();
            }
        }
    }
}
