using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTracker : MonoBehaviour
{
    [SerializeField]
    private readonly int MaxLaps = 3;
    public int Lap { get; private set; } = 0;
    [SerializeField]
    private TextMeshProUGUI lapCounter;
    [SerializeField]
    private TextMeshProUGUI countdownTimer;
    [SerializeField]
    private TrolleyPlayerController tp;
    private System.DateTime startTime;
    // Start is called before the first frame update
    void Awake()
    {
        tp.enabled = false;
        StartCoroutine("StartTimer");
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1);
        countdownTimer.text = "2";
        yield return new WaitForSeconds(1);
        countdownTimer.text = "1";
        yield return new WaitForSeconds(1);
        countdownTimer.text = "Go!";
        tp.enabled = true;
        startTime = System.DateTime.Now;
        yield return new WaitForSeconds(.5f);
        countdownTimer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Lap++;
        tp.SetLapBoost(Lap);
        if (Lap > MaxLaps)
        {
            System.TimeSpan difference = System.DateTime.Now - startTime;
            countdownTimer.text = string.Format("{00:00}", difference.Minutes) + ":" +
                string.Format("{00:00}", difference.Seconds) + ":" +
                string.Format("{00:00}", difference.Milliseconds);
            countdownTimer.gameObject.SetActive(true);
            tp.enabled = false;
        }
        else
        {
            lapCounter.text = "Lap: " + Lap + "/" + MaxLaps;
        }
    }
}
