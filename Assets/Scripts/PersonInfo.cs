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
    private float XRot;
    private void Start()
    {
        XRot = transform.eulerAngles.x;
    }
    public GameObject SetLocation(SpawnPoint spawnPoint)
    {
        weight = 0;
        if (!alive)
        {
            this.spawnPoint = spawnPoint;
            Vector3 spawnPos = new Vector3(spawnPoint.transform.position.x, 
                spawnPoint.transform.position.y + 8,
                spawnPoint.transform.position.z);
            transform.position = spawnPos;
            alive = true;
            gameObject.SetActive(true);
            return gameObject;
        }
        else
        {
            GameObject newChild = PersonSpawner.Instance.SpawnNew(this.gameObject);
            PersonInfo p = newChild.GetComponent<PersonInfo>();
            p.SetCopy();
            return p.SetLocation(spawnPoint);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Trolley"))
        {
            spawnPoint.Kill();
            other.gameObject.GetComponent<TrolleyPlayerController>().AddBoost(morality);
        }
    }

    public int GetWeight()
    {
        int weight = this.weight;
        this.weight = Mathf.Clamp(this.weight++, 0, 3);
        return weight;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(Camera.main.transform.position.x, transform.position.y,
            Camera.main.transform.position.z);
        transform.LookAt(targetPos);
        transform.eulerAngles = new Vector3(XRot, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
