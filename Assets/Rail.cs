using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rail : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPos = null;

    public Transform[] Lanes;

    [SerializeField]
    private TrolleyPlayerController playerController;

    private int currentLane = 0;

    bool canEnter = true;

    private Vector3 directionSwitch = Vector3.zero;

    [SerializeField]
    private Choice[] choices;
    [SerializeField]
    private ChoiceUI[] images;
    private int currentChoice = 0;

    private void Start()
    {
        if (Lanes[0].position.x == Lanes[1].position.x)
        {
            directionSwitch = new Vector3(int.MaxValue, 0, 1);
        }
        else
        {
            directionSwitch = new Vector3(1, 0, int.MaxValue);
        }
    }

    public void StartRail()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i < Lanes.Length)
                images[i].gameObject.SetActive(true);
            else
                images[i].gameObject.SetActive(false);
        }
        playerController.SetInitialForward(transform.forward);
        playerController.SetMovementState(TrolleyPlayerController.TrolleyMovementState.Railed);
        playerController.SetCameraPos(cameraPos);
        float shortestDist = float.MaxValue;
        int lanePos = 0;
        Vector3 trolleyPos = playerController.gameObject.transform.position;
        for (int i = 0; i < Lanes.Length; i++)
        {
            float dist = Mathf.Abs(Vector3.Distance(Lanes[i].position, trolleyPos));
            if (dist < shortestDist)
            {
                shortestDist = dist;
                lanePos = i;
            }
        }
        currentLane = lanePos;
        Vector3 newPos = new Vector3(Lanes[lanePos].position.x, trolleyPos.y, Lanes[lanePos].position.z);
        playerController.gameObject.transform.position = newPos;
    }

    public void DisplayChoices()
    {
        choices[currentChoice].FillImages(images);
    }

    public void SwitchLane(int i)
    {
        images[currentLane].LeaveLane();
        currentLane = Mathf.Clamp(currentLane + i, 0, Lanes.Length - 1);
        images[currentLane].EnterLane();
        Vector3 playerPos = playerController.gameObject.transform.position;
        Vector3 newPos = Vector3.zero;
        if (directionSwitch.x == int.MaxValue)
            newPos = new Vector3(playerPos.x, playerPos.y, Lanes[currentLane].position.z);
        else
            newPos = new Vector3(Lanes[currentLane].position.x, playerPos.y, playerPos.z);
        playerController.gameObject.transform.position = newPos;
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
            other.gameObject.GetComponent<TrolleyMoveState_Railed>().SetRail(this);
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
