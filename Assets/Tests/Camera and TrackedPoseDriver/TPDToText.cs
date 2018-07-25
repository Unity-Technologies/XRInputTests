using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SpatialTracking;

[RequireComponent (typeof(TrackedPoseDriver))]
public class TPDToText : MonoBehaviour {

    public TextMesh LabelText;

	// Use this for initialization
	void Start () {
        TrackedPoseDriver tpd = GetComponent<TrackedPoseDriver>();

        LabelText.text = tpd.deviceType.ToString() + ": " + tpd.poseSource.ToString();
	}
}
