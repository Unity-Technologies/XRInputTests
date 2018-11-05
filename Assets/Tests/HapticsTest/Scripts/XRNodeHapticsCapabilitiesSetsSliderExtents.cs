using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class XRNodeHapticsCapabilitiesSetsSliderExtents : MonoBehaviour
{
    public enum DriveType
    {
        Amplitude,
        Duration,
        Frequency
    }

    public XRNode node;
    public DriveType driveType;
    private Slider m_Slider;

    // Use this for initialization
    void Start()
    {
        m_Slider = GetComponent<Slider>();
        SetSliderExtents();
    }

    public void SetSliderExtents()
    {
        HapticCapabilities caps = new HapticCapabilities();
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (device == null
            || !device.TryGetHapticCapabilities(out caps) 
            || !caps.supportsImpulse
            )
            return;

        switch (driveType)
        {
            case DriveType.Amplitude:
                m_Slider.maxValue = 1f;
                m_Slider.minValue = 0;
                break;
            case DriveType.Frequency:
                m_Slider.maxValue = 1f;
                m_Slider.minValue = 0;
                break;
            case DriveType.Duration:
                m_Slider.maxValue = 10f;
                m_Slider.minValue = 0;
                break;
        }

        m_Slider.value = m_Slider.maxValue;
    }
}
