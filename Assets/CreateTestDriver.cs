using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTestDriver : MonoBehaviour
{
    public GameObject TestDriver;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("TestDriver") == null)
        {
            Debug.Log("Creating new test driver");
            Instantiate(TestDriver);
        }
    }
}
