using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HapticsStopButton : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(StopHaptics);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(StopHaptics);
    }

    private void StopHaptics()
    {
        Debug.Log("Sending Haptic Stop Command to both hands");
        InputDevice LeftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice RightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (LeftDevice != null)
            LeftDevice.StopHaptics();
        if (RightDevice != null)
            RightDevice.StopHaptics();
    }
}
