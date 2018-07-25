using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Wrapper around the concept of a supported platform, providing us with a logical
/// unit of the build target and build target group that's nicer to work with than
/// plain enums.
/// </summary>
public class XRPlatform
{
    static XRPlatform[] s_Platforms = new[]
    {
        new XRPlatform(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows),
        new XRPlatform(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64),
        new XRPlatform(BuildTargetGroup.Android, BuildTarget.Android),
        new XRPlatform(BuildTargetGroup.PS4, BuildTarget.PS4),
        new XRPlatform(BuildTargetGroup.iOS, BuildTarget.iOS),
    };

    readonly BuildTargetGroup m_Group;
    readonly BuildTarget m_Target;

    public BuildTarget buildTarget
    {
        get { return m_Target; }
    }

    XRPlatform(BuildTargetGroup group, BuildTarget target)
    {
        m_Group = group;
        m_Target = target;
    }

    /// <summary>
    /// Returns the currently active platform.
    /// </summary>
    public static XRPlatform currentPlatform
    {
        get
        {
            foreach (var plat in s_Platforms)
                if (plat.IsActive())
                    return plat;
            return null;
        }
    }

    /// <summary>
    /// Returns all valid platforms.
    /// </summary>
    public static XRPlatform[] allPlatforms
    {
        get { return s_Platforms; }
    }

    /// <summary>
    /// Gets the GUIContent objects for all platforms.
    /// </summary>
    public static GUIContent[] allPlatformsGuiContents
    {
        get { return s_Platforms.Select(p => new GUIContent(p.ToString())).ToArray(); }
    }

    /// <summary>
    /// Returns the first platform that supports the given BuildTarget.
    /// </summary>
    /// <param name="target">The build target of the platform to find.</param>
    /// <returns>The desired platform or null if no platform matches.</returns>
    public static XRPlatform GetPlatform(BuildTarget target)
    {
        foreach (var platform in s_Platforms)
            if (platform.m_Target == target)
                return platform;
        return null;
    }

    public void MakeActive()
    {
        if (!IsActive())
            EditorUserBuildSettings.SwitchActiveBuildTarget(m_Group, m_Target);
    }

    bool IsActive()
    {
        return m_Target == EditorUserBuildSettings.activeBuildTarget;
    }

    public override string ToString()
    {
        return ObjectNames.NicifyVariableName(m_Target.ToString());
    }
}
