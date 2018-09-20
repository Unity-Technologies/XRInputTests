using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;

public class XRInput : BaseInput
{
    [SerializeField, Tooltip("The keyboard key treated as the mouse button.")]
    KeyCode m_MouseKeyCode = KeyCode.F;

    public override bool mousePresent
    {
        get { return true; }
    }

    public override Vector2 mouseScrollDelta
    {
        get { return Vector2.zero; }
    }

    public override Vector2 mousePosition
    {
        get { return new Vector2(UnityEngine.XR.XRSettings.eyeTextureWidth / 2f, UnityEngine.XR.XRSettings.eyeTextureHeight / 2f); }
    }

    public override bool GetMouseButton(int button)
    {
        if (button != 0)
            return false;

        // Doesn't work on 18.3.0a9 - fix if necessary
        //if (Application.isMobilePlatform)
        //{

//#if UNITY_HAS_GOOGLEVR && !UNITY_IOS
//            if (GvrController.State == GvrConnectionState.Connected)
//            {
//                return GvrController.IsTouching;
//            }
//            else
//            {
//                return Input.GetMouseButton(0);
//            }
//#else
//            return Input.GetMouseButton(0);
//#endif
//        }
        return
            Input.GetButton("Button0") ||
            Input.GetButton("Button1") ||
            Input.GetButton("Button2") ||
            Input.GetButton("Button3") ||
            Input.GetButton("Button4") ||
            Input.GetButton("Button5") ||
            Input.GetButton("Button6") ||
            Input.GetButton("Button7") ||
            Input.GetButton("Button8") ||
            Input.GetButton("Button9") ||
            Input.GetButton("Button10") ||
            Input.GetButton("Button11") ||
            Input.GetButton("Button12") ||
            Input.GetButton("Button13") ||
            Input.GetButton("Button14") ||
            Input.GetButton("Button15") ||
            Input.GetButton("Button16") ||
            Input.GetButton("Button17") ||
            Input.GetButton("Button18") ||
            Input.GetButton("Button19")
        ;
    }

    public override bool GetMouseButtonDown(int button)
    {
        if (button != 0)
            return false;

        // Doesn't work on 18.3.0a9 - fix if necessary
//        if (Application.isMobilePlatform)
//        {
//#if UNITY_HAS_GOOGLEVR && !UNITY_IOS
//            if (GvrController.State == GvrConnectionState.Connected)
//            {
//                return GvrController.TouchDown;
//            }
//            else
//            {
//                return Input.GetMouseButtonDown(0);
//            }
//#else
//            return Input.GetMouseButtonDown(0);
//#endif
//        }
        return
            Input.GetButtonDown("Button0") ||
            Input.GetButtonDown("Button1") ||
            Input.GetButtonDown("Button2") ||
            Input.GetButtonDown("Button3") ||
            Input.GetButtonDown("Button4") ||
            Input.GetButtonDown("Button5") ||
            Input.GetButtonDown("Button6") ||
            Input.GetButtonDown("Button7") ||
            Input.GetButtonDown("Button8") ||
            Input.GetButtonDown("Button9") ||
            Input.GetButtonDown("Button10") ||
            Input.GetButtonDown("Button11") ||
            Input.GetButtonDown("Button12") ||
            Input.GetButtonDown("Button13") ||
            Input.GetButtonDown("Button14") ||
            Input.GetButtonDown("Button15") ||
            Input.GetButtonDown("Button16") ||
            Input.GetButtonDown("Button17") ||
            Input.GetButtonDown("Button18") ||
            Input.GetButtonDown("Button19")
        ;
    }

    public override bool GetMouseButtonUp(int button)
    {
        if (button != 0)
            return false;

        // Doesn't work on 18.3.0a9 - fix if necessary
//        if (Application.isMobilePlatform)
//        {
//#if UNITY_HAS_GOOGLEVR && !UNITY_IOS
//            if (GvrController.State == GvrConnectionState.Connected)
//            {
//                return GvrController.TouchUp;
//            }
//            else
//            {
//                return Input.GetMouseButtonUp(0);
//            }
//#else
//            return Input.GetMouseButtonUp(0);
//#endif
//        }
        return
            Input.GetButtonDown("Button0") ||
            Input.GetButtonDown("Button1") ||
            Input.GetButtonDown("Button2") ||
            Input.GetButtonDown("Button3") ||
            Input.GetButtonDown("Button4") ||
            Input.GetButtonDown("Button5") ||
            Input.GetButtonDown("Button6") ||
            Input.GetButtonDown("Button7") ||
            Input.GetButtonDown("Button8") ||
            Input.GetButtonDown("Button9") ||
            Input.GetButtonDown("Button10") ||
            Input.GetButtonDown("Button11") ||
            Input.GetButtonDown("Button12") ||
            Input.GetButtonDown("Button13") ||
            Input.GetButtonDown("Button14") ||
            Input.GetButtonDown("Button15") ||
            Input.GetButtonDown("Button16") ||
            Input.GetButtonDown("Button17") ||
            Input.GetButtonDown("Button18") ||
            Input.GetButtonDown("Button19")
        ;
    }

    public override bool GetButtonDown(string buttonName)
    {
        return false;
    }

    public override bool touchSupported
    {
        get { return false; }
    }
}
