using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class FunctionalTest : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            InputTracking.Recenter();
            Debug.Log("Orientation Recentered.");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            InputTracking.disablePositionalTracking = !InputTracking.disablePositionalTracking;
            Debug.Log("InputTracking.disablePositionalTracking set to " + InputTracking.disablePositionalTracking);
        }
	}
}
