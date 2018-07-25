using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class FloorOffset : MonoBehaviour {

    public Vector3 offsetAmount = Vector3.zero;

	// Use this for initialization
	void Start () {
		if (XRDevice.GetTrackingSpaceType() == TrackingSpaceType.Stationary)
        {
            transform.position += offsetAmount;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
