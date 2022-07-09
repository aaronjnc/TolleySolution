using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    private static PersonSpawner _instance;
    public static PersonSpawner Instance
    {
        get
        {
            return _instance;
        }
    }
    private Dictionary<int, List<PersonInfo>> people = new Dictionary<int, List<PersonInfo>>();
    [SerializeField]
    private Transform personParent;
    private void Awake()
    {
        _instance = this;
        Object[] peopleObjects = Resources.LoadAll("CharacterPrefabs", typeof(GameObject));
        foreach (Object obj in peopleObjects)
        {
            GameObject person = SpawnNew(obj);
            person.SetActive(false);
            PersonInfo personInfo = person.GetComponent<PersonInfo>();
            if (!people.ContainsKey(personInfo.GetMorality()))
            {
                people.Add(personInfo.GetMorality(), new List<PersonInfo>());
            }
            people[personInfo.GetMorality()].Add(personInfo);
        }
    }

    public GameObject SpawnNew(Object personToSpawn)
    {
        return (GameObject)Instantiate(personToSpawn, personParent);
    }

    public GameObject[] GetPeople(int number)
    {
        List<GameObject> chosen = new List<GameObject>();
        List<int> moralities = new List<int>();
        for (int i = 0; i < number; i++)
        {
            float highest = 0;
            int morality = 0;
            foreach (int key in people.Keys)
            {
                if (moralities.Contains(key))
                    continue;
                float random = Random.Range(0f, 1f);
                if (random > highest)
                    morality = key;
            }
            highest = 0;
            moralities.Add(morality);
            GameObject current = null;
            foreach (PersonInfo person in people[morality])
            {
                if (chosen.Contains(person.gameObject))
                    continue;
                float random = person.GetWeight() * Random.Range(0f, 1f);
                if (random > highest)
                    current = person.gameObject;
            }
            chosen.Add(current);
        }
        return chosen.ToArray();
    }
}
