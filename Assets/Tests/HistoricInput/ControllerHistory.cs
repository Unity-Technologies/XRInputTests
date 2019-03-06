using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.XR;

public class ControllerHistory : BasePoseProvider
{
    public int TimeBackwards = 50;
    public XRNode DeviceToTrack;

    public override bool TryGetPoseFromProvider(out Pose output)
    {
        var device = InputDevices.GetDeviceAtXRNode(DeviceToTrack);
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        DateTime time = new DateTime();
        time = DateTime.Now;
        time.AddMilliseconds(-TimeBackwards);

        if(device.TryGetFeatureValue(CommonUsages.deviceRotation, time, out rotation))
        {
            device.TryGetFeatureValue(CommonUsages.devicePosition, time, out position);          
            output.position = position;
            output.rotation = rotation;
            return true;            
        }        
        else
        {
            output = Pose.identity;
            return false;
        }
    }
}
