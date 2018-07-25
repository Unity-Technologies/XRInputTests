using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Linq;
using System;
using UnityEngine.SpatialTracking;

public class NodeStateDisplay : MonoBehaviour
{
    public XRNode m_Node;

    public Text NodeTitle_Text;

    [Header("Translation Information")]
    public Image Position_Image;
    public Text Position_Text;
    public Image Velocity_Image;
    public Text Velocity_Text;
    public Image Acceleration_Image;
    public Text Acceleration_Text;

    [Header("Rotation Information")]
    public Image Rotation_Image;
    public Text Rotation_Text;
    public Image AngularVelocity_Image;
    public Text AngularVelocity_Text;
    public Image AngularAcceleration_Image;
    public Text AngularAcceleration_Text;

    void Awake()
    {
    }
    
    void Update ()
    {
        if (NodeTitle_Text != null)
        {
            NodeTitle_Text.text = m_Node.ToString();
        }

        var nodeStates = new List<XRNodeState>();
        UnityEngine.XR.InputTracking.GetNodeStates(nodeStates);

        XRNodeState? state = null;
        
        foreach(XRNodeState nodeState in nodeStates)
        {
            if( nodeState.nodeType == m_Node)
            {
                state = nodeState;
                break;
            }
        }
        
        if(state.HasValue)
        {
            XRNodeState node = state.Value;
            Vector3 tempVector;
            Quaternion tempQuaternion;

            // Translation Information
            SetImageColor(Position_Image, node.TryGetPosition(out tempVector));
            Position_Text.text = Vector3ToFieldText(tempVector);
            SetImageColor(Velocity_Image, node.TryGetVelocity(out tempVector));
            Velocity_Text.text = Vector3ToFieldText(tempVector);
            SetImageColor(Acceleration_Image, node.TryGetAcceleration(out tempVector));
            Acceleration_Text.text = Vector3ToFieldText(tempVector);

            // Rotation Information
            SetImageColor(Rotation_Image, node.TryGetRotation(out tempQuaternion));
            Rotation_Text.text = QuaternionToFieldText(tempQuaternion);
            SetImageColor(AngularVelocity_Image, node.TryGetAngularVelocity(out tempVector));
            AngularVelocity_Text.text = Vector3ToFieldText(tempVector);
            SetImageColor(AngularAcceleration_Image, node.TryGetAngularAcceleration(out tempVector));
            AngularAcceleration_Text.text = Vector3ToFieldText(tempVector);
        }
        else
        {
            // Translation Information
            SetImageColor(Position_Image, false);
            SetImageColor(Velocity_Image, false);
            SetImageColor(Acceleration_Image, false);

            // Rotation Information
            SetImageColor(Rotation_Image, false);
            SetImageColor(AngularVelocity_Image, false);
            SetImageColor(AngularAcceleration_Image, false);
        }

    }

    private string Vector3ToFieldText (Vector3 inVec)
    {
        return "x = " + inVec.x.ToString("+000.000; -000.000; +000.000")
            + "\ny = " + inVec.y.ToString("+000.000; -000.000; +000.000")
            + "\nz = " + inVec.z.ToString("+000.000; -000.000; +000.000");
    }

    private string QuaternionToFieldText(Quaternion inQuat)
    {
        return "w = " + inQuat.w.ToString("+000.000; -000.000; +000.000")
            + "\nx = " + inQuat.x.ToString("+000.000; -000.000; +000.000")
            + "\ny = " + inQuat.y.ToString("+000.000; -000.000; +000.000")
            + "\nz = " + inQuat.z.ToString("+000.000; -000.000; +000.000");
    }

    private void SetImageColor(Image image, bool state)
    {
        if (image != null)
        {
            image.color = state ? Color.green : Color.red;
        }
    }
}
