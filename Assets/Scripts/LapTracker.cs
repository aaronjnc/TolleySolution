using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;

public class LapTracker : MonoBehaviour
{
    [SerializeField]
    private int MaxLaps = 3;
    public int Lap { get; private set; } = 0;
    [SerializeField]
    private TextMeshProUGUI lapCounter;
    [SerializeField]
    private TextMeshProUGUI countdownTimer;
    [SerializeField]
    private TrolleyPlayerController tp;
    private System.DateTime startTime;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject quitButton;
    [SerializeField]
    private GameObject highscoreList;
    [SerializeField]
    private TextMeshProUGUI[] highScores;
    private HighScores highScoreScript;
    [SerializeField]
    private GameObject clearScores;
    private TimeSpan finalTime;
    [SerializeField]
    private TextMeshProUGUI gameTime;
    private bool IsPlaying = false;
    // Start is called before the first frame update
    void Awake()
    {
        highScoreScript = new HighScores();
        tp.enabled = false;
        StartCoroutine("StartTimer");
    }

    private void Update()
    {
        if (IsPlaying)
        {
            gameTime.text = TimeString(DateTime.Now - startTime);
        }    
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
        startTime = DateTime.Now;
        IsPlaying = true;
        yield return new WaitForSeconds(.5f);
        countdownTimer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Lap++;
        tp.SetLapBoost(Lap);
        if (Lap > MaxLaps)
        {
            IsPlaying = false;
            finalTime = DateTime.Now - startTime;
            DisplayScores(finalTime);
            restartButton.SetActive(true);
            quitButton.SetActive(true);
            tp.enabled = false;
            Time.timeScale = 0;
        }
        else
        {
            lapCounter.text = "Lap: " + Lap + "/" + MaxLaps;
        }
    }

    private string TimeString(TimeSpan difference)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format("{00:00}", difference.Minutes));
        sb.Append(":");
        sb.Append(string.Format("{00:00}", difference.Seconds));
        sb.Append(":");
        sb.Append(string.Format("{00:00}", difference.Milliseconds));
        return sb.ToString();
    }

    private void DisplayScores(TimeSpan time)
    {
        gameTime.text = TimeString(time);
        int spot = highScoreScript.AddTime(time);
        TimeSpan[] times = highScoreScript.GetTimes();
        for (int i = 0; i < times.Length && i < highScores.Length; i++)
        {
            highScores[i].text = (i + 1) + ": " + TimeString(times[i]);
            if (i == spot)
            {
                highScores[i].color = Color.red;
            }
            else
            {
                highScores[i].color = Color.black;
            }
        }
        highscoreList.SetActive(true);
        clearScores.SetActive(true);
        highScoreScript.SaveTimes();
    }

    public void ResetHighScores()
    {
        string path = Path.Combine(Application.persistentDataPath, "highScores.txt");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        highScoreScript = new HighScores();
        DisplayScores(finalTime);
    }
}
