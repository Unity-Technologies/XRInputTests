using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class XRNodeCapabilities : MonoBehaviour {

    public XRNode node;

    public Text nodeCheck;
    public Text numChannels;
    public Text supportsImpulse;
    public Text supportsBuffer;
    public Text bufferFreqHz;
    public Text bufferMaxSize;
    
    
	void Update () {
        HapticCapabilities caps = new HapticCapabilities();
        InputHaptic.TryGetCapabilities(node, out caps);

        nodeCheck.text = node.ToString();
        numChannels.text = caps.numChannels.ToString();
        supportsImpulse.text = caps.supportsImpulse.ToString();
        supportsBuffer.text = caps.supportsBuffer.ToString();
        bufferFreqHz.text = caps.bufferFrequencyHz.ToString();
        bufferMaxSize.text = caps.bufferMaxSize.ToString();
	}
}
