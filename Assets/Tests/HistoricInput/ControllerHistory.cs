using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.XR;

using UnityEngine.SpatialTracking;

public class ControllerHistory : BasePoseProvider
{
    public double TimeBackwards = 50;
    public XRNode DeviceToTrack;

    public override PoseDataFlags GetPoseFromProvider(out Pose output)
    {
        var device = InputDevices.GetDeviceAtXRNode(DeviceToTrack);
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        DateTime time = new DateTime();
        time = DateTime.Now;
        time = time.AddMilliseconds(-TimeBackwards);

        if(device.TryGetFeatureValue(CommonUsages.deviceRotation, time, out rotation))
        {
            Debug.Log("Frame " + Time.frameCount + " " + DeviceToTrack + " rotation at time " + DateTime.Now.Millisecond + " - " + TimeBackwards + "= (" + time.Millisecond + ") is " + rotation.w + ", " + rotation.x + ", " + rotation.y + ", " + rotation.z);

            device.TryGetFeatureValue(CommonUsages.devicePosition, time, out position);          
            output.position = position;
            output.rotation = rotation;
            return PoseDataFlags.Position | PoseDataFlags.Rotation;            
        }        
        else
        {
            output = Pose.identity;
            return PoseDataFlags.NoData;
        }
    }
}
