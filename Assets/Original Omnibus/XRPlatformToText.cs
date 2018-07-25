using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class XRPlatformToText : MonoBehaviour {

    public Text loadedDeviceText;
    public Text supportedDeviceText;
    public Text joystickNamesText;
    public Text eventsText;

    private Queue<string> m_Events;
    private int m_QueueMaximumSize = 14;
    private int m_EventNumber = 0;

    private void Awake()
    {
        // Queue setup
        m_Events = new Queue<string>();
    }

    // Use this for initialization
    void Start () {
        string displayTextAccumulator;

        // Loaded device
        loadedDeviceText.text = XRSettings.loadedDeviceName;
        
        // Supported devices (this build)
        displayTextAccumulator = "";
        foreach (string device in XRSettings.supportedDevices)
        {
            displayTextAccumulator += device + ", ";
        }
        supportedDeviceText.text = displayTextAccumulator;

        // Joystick names
        ShowJoystickNames();
    }

    private void OnEnable()
    {
        InputTracking.nodeAdded += OnNodeAdded;
        InputTracking.nodeRemoved += OnNodeRemoved;
        InputTracking.trackingAcquired += OnTrackingAcquired;
        InputTracking.trackingLost += OnTrackingLost;
    }

    private void OnDisable()
    {
        InputTracking.nodeAdded -= OnNodeAdded;
        InputTracking.nodeRemoved -= OnNodeRemoved;
        InputTracking.trackingAcquired -= OnTrackingAcquired;
        InputTracking.trackingLost -= OnTrackingLost;
    }

    
    void OnNodeAdded (XRNodeState NodeState)
    {
        AddEventToQueue("NodeAdded: " + NodeState.nodeType);
        ShowJoystickNames();
    }
    void OnNodeRemoved (XRNodeState NodeState)
    {
        AddEventToQueue("NodeRemoved: " + NodeState.nodeType);
        ShowJoystickNames();
    }
    void OnTrackingAcquired (XRNodeState NodeState)
    {
        AddEventToQueue("TrackingAquired: " + NodeState.nodeType);
    }
    void OnTrackingLost (XRNodeState NodeState)
    {
        AddEventToQueue("TrackingLost: " + NodeState.nodeType);
    }

    void AddEventToQueue(string EventDescriptor)
    {
        string displayTextAccumulator = "";

        EventDescriptor = "<" + m_EventNumber + "> " + EventDescriptor;
        m_EventNumber++;
        m_Events.Enqueue(EventDescriptor);
        
        while (m_Events.Count > m_QueueMaximumSize)
        {
            m_Events.Dequeue();
        }

        // Print events
        foreach (string eventString in m_Events)
        {
            displayTextAccumulator = (eventString + "\n") + displayTextAccumulator;
        }
        eventsText.text = displayTextAccumulator;
    }

    void ShowJoystickNames()
    {
        string displayTextAccumulator = "";
        int joystickNumber = 0;

        foreach (string device in Input.GetJoystickNames())
        {
            displayTextAccumulator += ("<" + joystickNumber + "> " + device + "\n");
            joystickNumber++;
        }
        joystickNamesText.text = displayTextAccumulator;
    }
}
