using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] private GameObject[] RightSparks;
    [SerializeField] private GameObject[] LeftSparks;

    public void EnableRightSparks(bool enable = true )
    {
        for (int i = 0; i < RightSparks.Length; i++)
        {
            RightSparks[i].SetActive(enable);
        }
    }

    public void EnableLeftSparks(bool enable = true)
    {
        for (int i = 0; i < LeftSparks.Length; i++)
        {
            LeftSparks[i].SetActive(enable);
        }
    }

}
