using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class DutyCycle_50 : ClipTestButton
{
    protected override bool GenerateClip(XRNode node, ref byte[] clip)
    {
        HapticCapabilities caps = new HapticCapabilities();

        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (device == null
            || !device.TryGetHapticCapabilities(out caps)
            )
            return false;

        // Generate actual clip
        int clipTime = (int)(caps.bufferFrequencyHz * 2); // 2 seconds
        clip = new byte[clipTime];
        for (int i = 0; i < clipTime; i++)
        {
            clip[i] = (i % 2 == 0) ? byte.MaxValue : (byte)0;
        }

        return true;
    }
}
