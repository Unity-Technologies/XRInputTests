using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClipTestButton : MonoBehaviour {

    private byte[] m_LeftClip = null;
    private byte[] m_RightClip = null;
	
	// Update is called once per frame
	void OnEnable () {
        GetComponent<Button>().onClick.AddListener(PlayClip);
	}

    void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(PlayClip);
    }

    // In implementations, overwrite this to generate clips
    protected virtual bool GenerateClip(XRNode node, ref byte[] clip)
    {
        HapticCapabilities caps = new HapticCapabilities();

        if (!InputHaptic.TryGetCapabilities(node, out caps))
            return false;

        // This base implementation generates a very boring clip of solid intensity
        // over the max clip time.
        clip = new byte[caps.bufferMaxSize];
        for(int i = 0; i < caps.bufferMaxSize; i++)
        {
            clip[i] = byte.MaxValue;
        }

        return true;
    }

    void PlayClip()
    {
        if (GenerateClip(XRNode.LeftHand, ref m_LeftClip))
            InputHaptic.SendBuffer(XRNode.LeftHand, 0, m_LeftClip);
        if (GenerateClip(XRNode.RightHand, ref m_RightClip))
            InputHaptic.SendBuffer(XRNode.RightHand, 0, m_RightClip);
    }
}
