using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public static class XRBuildSettings
{
    public static void UpdateBuildSettings()
    {
        var currentIndex = Array.IndexOf(XRPlatform.allPlatforms, XRPlatform.currentPlatform);
        if (currentIndex < 0)
            currentIndex = 0;

        var xrTestSuiteConfiguration = new XRTestSuiteConfiguration
        {
            buildTarget = XRPlatform.allPlatforms[currentIndex].buildTarget,
            renderPath = ValidRenderingPaths()[CurrentRenderPathIndex()],
            stereoRenderingPath = PlayerSettings.stereoRenderingPath,
            stereoRendering = true,
            xrSdk = XRSdkDescription.currentSdk
        };

        var scenes = new List<string> { SceneHelper.StartScene };
        scenes.AddRange(SceneHelper.GetTestScenePaths().Where(xrTestSuiteConfiguration.ShouldBuildTestScene));
        EditorBuildSettings.scenes = scenes.Select(s => new EditorBuildSettingsScene {enabled = true, path = s}).ToArray();
        AssetDatabase.SaveAssets();
    }

    public static List<RenderingPath> ValidRenderingPaths()
    {
        var renderingPaths = Enum.GetValues(typeof(RenderingPath)).Cast<RenderingPath>().ToList();
        renderingPaths.Remove(RenderingPath.UsePlayerSettings);
        return renderingPaths;
    }

    public static int CurrentRenderPathIndex()
    {
        var currentRenderingPath =
            EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier3).renderingPath;
        return ValidRenderingPaths().IndexOf(currentRenderingPath);
    }
}
