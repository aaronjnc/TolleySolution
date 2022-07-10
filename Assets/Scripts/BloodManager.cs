using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    [SerializeField] private GameObject[] BloodBank;

    float time = -1f;
    float maxBloodTime = 2f;
    public void GetBlood(Vector3 pos)
    {
        time = Time.time;
        for (int i =0; i < BloodBank.Length; i++)
        {
            if (!BloodBank[i].activeInHierarchy)
            {
                BloodBank[i].transform.position = pos;
                BloodBank[i].SetActive(true);
                return;
            }
            
        }

        for (int i = 0; i < BloodBank.Length; i++)
        {
            if (BloodBank[i].activeInHierarchy)
            {
                BloodBank[i].transform.position = pos;
                return;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((time - Time.time) > float.Epsilon)
        {
            for (int i = 0; i < BloodBank.Length; i++)
            {
                if (BloodBank[i].activeInHierarchy)
                {
                    BloodBank[i].SetActive(false);
                }

            }
        }
    }
}
