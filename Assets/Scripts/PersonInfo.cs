using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInfo : MonoBehaviour {
    [SerializeField]
    private string personName = "";
    [SerializeField]
    private int morality = 0;
    private bool alive = false;
    private bool original = true;
    private SpawnPoint spawnPoint;
    private int weight = 3;
    public void SetLocation(SpawnPoint spawnPoint)
    {
        weight = 0;
        if (!alive)
        {
            this.spawnPoint = spawnPoint;
            transform.position = spawnPoint.transform.position;
            alive = true;
            gameObject.SetActive(true);
        }
        else
        {
            GameObject newChild = PersonSpawner.Instance.SpawnNew(this.gameObject);
            PersonInfo p = newChild.GetComponent<PersonInfo>();
            p.SetCopy();
            p.SetLocation(spawnPoint);
        }
    }

    public void SetCopy()
    {
        original = false;
    }
    public string GetPersonName()
    {
        return personName;
    }
    public int GetMorality()
    {
        return morality;
    }
    public void Kill()
    {
        if (original)
        {
            gameObject.SetActive(false);
            alive = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if trolley
        spawnPoint.Kill();
    }
    public int GetWeight()
    {
        int weight = this.weight;
        this.weight = Mathf.Clamp(this.weight++, 0, 3);
        return weight;
    }
}
