using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class NodeEventsToText : MonoBehaviour {
    
    public Text eventsText;
    public int m_QueueMaximumSize = 15;
    [Tooltip("After the queue is full, no longer accept new values.")]
    public bool fillQueueOnlyOnce = false;

    private Queue<string> m_Events;
    private int m_EventNumber = 0;

    private void Awake()
    {
        // Queue setup
        m_Events = new Queue<string>();
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
    }
    void OnNodeRemoved (XRNodeState NodeState)
    {
        AddEventToQueue("NodeRemoved: " + NodeState.nodeType);
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
        if (fillQueueOnlyOnce && (m_EventNumber >= m_QueueMaximumSize))
        {
            return;
        }

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
}
