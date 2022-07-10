using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [SerializeField]
    private SpawnPoint[] spawnPoints;
    [SerializeField]
    private float WaitTime = 3f;
    private ChoiceUI[] images;
    private Rail rail;
    private void Start()
    {
        rail = GetComponentInParent<Rail>();
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
        rail.DisplayChoices();
        StartCoroutine("WaitToSpawn");
    }

    public IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(WaitTime);
        SpawnPeople();
    }

    public void FillImages(ChoiceUI[] images)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            PersonInfo person = spawnPoints[i].GetPersonInfo();
            Texture image = person.GetComponent<MeshRenderer>().material.mainTexture;
            images[i].SetUp(image, person.GetPersonName());
        }
    }
}
