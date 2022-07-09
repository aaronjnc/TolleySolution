using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPos = null;

    public Transform[] Lanes;

    [SerializeField]
    private TrolleyPlayerController playerController;

    private int currentLane = 0;

    bool canEnter = true;

    public void StartRail()
    {
        playerController.SetInitialForward(transform.forward);
        playerController.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Railed);
        playerController.SetCameraPos(cameraPos);
        float shortestDist = float.MaxValue;
        int lanePos = 0;
        Vector3 trolleyPos = playerController.gameObject.transform.position;
        for (int i = 0; i < Lanes.Length; i++)
        {
            float dist = Vector3.Distance(Lanes[i].position, trolleyPos);
            if (dist < shortestDist)
            {
                lanePos = i;
            }
        }
        Vector3 newPos = new Vector3(Lanes[lanePos].position.x, trolleyPos.y, Lanes[lanePos].position.z);
        playerController.gameObject.transform.position = newPos;
    }

    public void SwitchLane(int i)
    {
        currentLane = Mathf.Clamp(currentLane + i, 0, Lanes.Length - 1);
        playerController.gameObject.transform.position = Lanes[currentLane].position;
    }

    public void StopRail()
    {
        playerController.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Free);
        playerController.ResetCamera();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canEnter && other.gameObject.Equals(playerController.gameObject))
        {
            StartRail();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(playerController.gameObject))
        {
            StopRail();
            StartCoroutine("ExitWait");
        }
    }

    IEnumerator ExitWait()
    {
        canEnter = false;
        yield return new WaitForSeconds(1f);
        canEnter = true;
    }
}
