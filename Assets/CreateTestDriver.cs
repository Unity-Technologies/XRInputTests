using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTestDriver : MonoBehaviour
{
    public GameObject TestDriver;
    public GameObject TestDriver_AR;

    public bool AR;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("TestDriver") == null)
        {
            Debug.Log("Creating new test driver");
            if (AR)
                Instantiate(TestDriver_AR);
            else
                Instantiate(TestDriver);
        }
    }
}
