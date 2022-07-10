using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores
{
    private System.TimeSpan[] topTimes = new System.TimeSpan[5];

    public HighScores()
    {
        string path = Path.Combine(Application.persistentDataPath, "highScores.txt");
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            int i = 0;
            foreach (string line in lines)
            {
                int min = 0;
                int sec = 0;
                int mill = 0;
                string[] values = line.Split(':');
                min = int.Parse(values[0]);
                sec = int.Parse(values[1]);
                mill = int.Parse(values[2]);
                topTimes[i] = new System.TimeSpan(0, 0, min, sec, mill);
                i++;
            }
        }
    }

    public int AddTime(System.TimeSpan time)
    {
        int spot = -1;
        System.TimeSpan replaced = System.TimeSpan.Zero;
        for (int i = 0; i < topTimes.Length; i++)
        {
            if (time.CompareTo(topTimes[i]) < 0 || topTimes[i] == System.TimeSpan.Zero)
            {
                spot = i;
                replaced = topTimes[i];
                topTimes[i] = time;
                break;
            }
        }
        if (spot == -1 || replaced == System.TimeSpan.Zero)
            return spot;
        for (int i = spot + 1; i < topTimes.Length; i++)
        {
            System.TimeSpan temp = topTimes[i];
            topTimes[i] = replaced;
            replaced = temp;
        }
        return spot;
    }

    public System.TimeSpan[] GetTimes()
    {
        return topTimes;
    }

    public string TimeSpanString(System.TimeSpan time)
    {
        return string.Format("{00:00}", time.Minutes) + ":" +
                string.Format("{00:00}", time.Seconds) + ":" +
                string.Format("{00:00}", time.Milliseconds);
    }

    public string[] GetLines()
    {
        string[] lines = new string[topTimes.Length];
        for (int i = 0; i < topTimes.Length; i++)
        {
            lines[i] = (i + 1) + ": " + TimeSpanString(topTimes[i]);
        }
        return lines;
    }

    public void SaveTimes()
    {
        string path = Path.Combine(Application.persistentDataPath, "highScores.txt");
        string[] lines = GetLines();
        File.WriteAllLines(path, lines);
    }
}
