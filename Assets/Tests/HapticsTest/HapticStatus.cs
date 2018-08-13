using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class HapticStatus : MonoBehaviour {

    public XRNode node;

    public Text nodeCheck;
    public Text samplesAvailable;
    public Text samplesQueued;
	
	// Update is called once per frame
	void Update () {
        HapticState state = new HapticState();
        InputHaptic.TryGetState(node, out state);

		nodeCheck.text = node.ToString();
        samplesAvailable.text = state.samplesAvailable.ToString();
        samplesQueued.text = state.samplesQueued.ToString();
	}
}
