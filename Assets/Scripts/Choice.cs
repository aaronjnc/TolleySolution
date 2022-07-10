using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    [SerializeField]
    private SpawnPoint[] spawnPoints;
    [SerializeField]
    private float WaitTime = 3f;
    private void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].SetChoice(this);
        }
        SpawnPeople();
    }

    private void SpawnPeople()
    {
        GameObject[] people = PersonSpawner.Instance.GetPeople(spawnPoints.Length);
        for (int i = 0; i < spawnPoints.Length && i < people.Length; i++)
        {
            spawnPoints[i].FillSpace(people[i]);
        }
    }

    public void DisablePeople()
    {
        StartCoroutine("WaitToSpawn");
    }

    public IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(WaitTime);
        SpawnPeople();
    }
}
