using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal.VR;
using UnityEngine;

/// <summary>
/// Wrapper around the concept of an SDK for our test suite.
/// </summary>
public class XRSdkDescription
{
    public static readonly XRSdkDescription s_Oculus = new XRSdkDescription("Oculus", "Oculus");
    public static readonly XRSdkDescription s_OpenVr = new XRSdkDescription("OpenVR", "OpenVR");
    public static readonly XRSdkDescription s_PlaystationVr = new XRSdkDescription("PlaystationVR", "Morpheus");
    public static readonly XRSdkDescription s_Cardboard = new XRSdkDescription("Cardboard", "cardboard");
    public static readonly XRSdkDescription s_Daydream = new XRSdkDescription("Daydream", "daydream");

    /// <summary>
    /// Must match each build target group to the set of valid SDKs for that platform. Used to make our configuration
    /// window not show invalid settings for the build target group. When switching groups the first SDK in the array
    /// is used as the default for that group if the current SDK is invalid.
    /// </summary>
    static readonly Dictionary<BuildTargetGroup, XRSdkDescription[]> s_SupportedSdks = new Dictionary<BuildTargetGroup, XRSdkDescription[]>
    {
        {BuildTargetGroup.Standalone, new[] {s_Oculus, s_OpenVr}},
        {BuildTargetGroup.Android, new[] {s_Oculus, s_Cardboard, s_Daydream}},
        {BuildTargetGroup.iOS, new[] {s_Cardboard}},
        {BuildTargetGroup.PS4, new[] {s_PlaystationVr}},
    };

    readonly string m_Name;
    readonly string m_Key;

    public GUIContent guiContent
    {
        get { return new GUIContent(m_Name); }
    }

    XRSdkDescription(string name, string key)
    {
        m_Name = name;
        m_Key = key;
    }

    /// <summary>
    /// Enables this SDK for the current build target group.
    /// </summary>
    public void EnableForCurrentBuildTargetGroup()
    {
        var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        VREditor.SetVREnabledOnTargetGroup(buildTargetGroup, true);

        // Build an array with our key first followed by all other supported SDKs
        var otherSdkKeys = s_SupportedSdks[buildTargetGroup].Except(new[] {this}).Select(s => s.m_Key);
        VREditor.SetVREnabledDevicesOnTargetGroup(buildTargetGroup, new[] {m_Key}.Concat(otherSdkKeys).ToArray());
    }

    /// <summary>
    /// Determines if this SDK is active for the current build target group.
    /// </summary>
    /// <returns>True if this is the active SDK, false otherwise.</returns>
    public bool IsActiveSdk()
    {
        var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var chosenSdk = VREditor.GetVREnabledDevicesOnTargetGroup(buildTargetGroup).FirstOrDefault();
        return chosenSdk == m_Key;
    }

    /// <summary>
    /// Returns all valid XR SDKs for the current build target group.
    /// </summary>
    public static XRSdkDescription[] validSdks
    {
        get
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            return s_SupportedSdks[buildTargetGroup];
        }
    }

    /// <summary>
    /// Returns the GUIContent objects for the valid SDKs for the current build target group.
    /// </summary>
    public static GUIContent[] validSdkGuiContents
    {
        get { return validSdks.Select(s => s.guiContent).ToArray(); }
    }

    /// <summary>
    /// Returns the currently active SDK, or null if no SDK is active for the current build target group.
    /// </summary>
    public static XRSdkDescription currentSdk
    {
        get { return validSdks.FirstOrDefault(validSdk => validSdk.IsActiveSdk()); }
    }
}
