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
        InputDevice LeftDevice = new InputDevice();
        InputDevice RightDevice = new InputDevice();

        if (InputDevices.TryGetDeviceAtXRNode(XRNode.LeftHand, out LeftDevice))
            LeftDevice.StopHaptics();
        if (InputDevices.TryGetDeviceAtXRNode(XRNode.RightHand, out RightDevice))
            RightDevice.StopHaptics();
    }
}
