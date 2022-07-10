using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Choice choice;
    private bool occupied = false;
    private GameObject person;
    public void SetChoice(Choice choice)
    {
        this.choice = choice;
    }

    public void Kill()
    {
        person.GetComponent<PersonInfo>().Kill();
        occupied = false;
        choice.DisablePeople();
    }

    public void DisablePerson()
    {
        if (!occupied) return;
        person.GetComponent<PersonInfo>().Kill();
        occupied = false;
    }

    public void FillSpace(GameObject person)
    {
        if (occupied)
        {
            DisablePerson();
        }
        this.person = person.GetComponent<PersonInfo>().SetLocation(this);
        occupied = true;
    }

    public PersonInfo GetPersonInfo()
    {
        return person.GetComponent<PersonInfo>();
    }
}
