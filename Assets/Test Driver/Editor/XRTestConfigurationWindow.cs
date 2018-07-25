using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
public class XRTestConfigurationWindow : EditorWindow, UnityEditor.Build.IActiveBuildTargetChanged
{

    private static XRTestConfigurationWindow self;

    void OnEnable()
    {
        self = this;
        PlayerSettings.virtualRealitySupported = true;
        titleContent = new GUIContent("XR Test Configuration");
    }

    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        Repaint();
    }

    void OnGUI()
    {
        PlatformSelector();
        SdkSelector();
        RenderingPathSelector();
        StereoRenderingPathSelector();
        GraphicsJobsToggle();
        UpdateBuildSettings();
    }

    public static void PlatformSelector()
    {
        EditorGUI.BeginChangeCheck();
        var currentIndex = Array.IndexOf(XRPlatform.allPlatforms, XRPlatform.currentPlatform);
        if (currentIndex < 0)
            currentIndex = 0;

        var guiContent = new GUIContent("Platform", "Choose the platform to select for the build settings.");
        var newIndex = EditorGUILayout.Popup(guiContent, currentIndex, XRPlatform.allPlatformsGuiContents);
        if (EditorGUI.EndChangeCheck())
        {
            XRPlatform.allPlatforms[newIndex].MakeActive();
        }
    }

    public static void StereoRenderingPathSelector()
    {
        EditorGUI.BeginChangeCheck();
        var guiContent = new GUIContent("Stereo Rendering Path", "Rendering path specific to XR.");
        var newStereoPath = (StereoRenderingPath)EditorGUILayout.EnumPopup(guiContent, PlayerSettings.stereoRenderingPath);

        if (EditorGUI.EndChangeCheck())
            PlayerSettings.stereoRenderingPath = newStereoPath;
    }

    public static void RenderingPathSelector()
    {
        var currentPathIndex = XRBuildSettings.CurrentRenderPathIndex();

        EditorGUI.BeginChangeCheck();
        var guiContent = new GUIContent("Rendering Path",
                "The rendering path to use the project. Applied to all graphics tiers.");
        var newRenderingPathIndex = EditorGUILayout.Popup(guiContent, currentPathIndex, ValidRenderingPathsGuiContents());
        if (EditorGUI.EndChangeCheck())
        {
            var newRenderingPath = XRBuildSettings.ValidRenderingPaths()[newRenderingPathIndex];
            foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                foreach (GraphicsTier tier in Enum.GetValues(typeof(GraphicsTier)))
                {
                    var settings = EditorGraphicsSettings.GetTierSettings(group, tier);
                    settings.renderingPath = newRenderingPath;
                    EditorGraphicsSettings.SetTierSettings(group, tier, settings);
                }
            }
        }
    }

    public static void GraphicsJobsToggle()
    {
        EditorGUI.BeginChangeCheck();
        var guiContent = new GUIContent("Graphics Jobs", "Toggles the graphics jobs player setting.");
        var newGraphicsJobs = EditorGUILayout.Toggle(guiContent, PlayerSettings.graphicsJobs);
        if (EditorGUI.EndChangeCheck())
            PlayerSettings.graphicsJobs = newGraphicsJobs;
    }

    public static void DevelopmentToggle()
    {
        EditorGUI.BeginChangeCheck();
        var guiContent = new GUIContent("Development Build", "Toggles the development user build setting.");
        var newDevelopmentBuild = EditorGUILayout.Toggle(guiContent, EditorUserBuildSettings.development);
        if (EditorGUI.EndChangeCheck())
            EditorUserBuildSettings.development = newDevelopmentBuild;
    }

    static GUIContent[] ValidRenderingPathsGuiContents()
    {
        return XRBuildSettings.ValidRenderingPaths().Select(p => new GUIContent(p.ToString())).ToArray();
    }

    void UpdateBuildSettings()
    {
        if (GUILayout.Button("Update Build Settings"))
            XRBuildSettings.UpdateBuildSettings();
    }

    public static void SdkSelector()
    {
        EditorGUI.BeginChangeCheck();
        var currentSdkIndex = Array.IndexOf(XRSdkDescription.validSdks, XRSdkDescription.currentSdk);
        var index = EditorGUILayout.Popup(new GUIContent("SDK", "SDK used for testing"), currentSdkIndex,
                XRSdkDescription.validSdkGuiContents);

        if (EditorGUI.EndChangeCheck())
        {
            var sdks = XRSdkDescription.validSdks;
            sdks[index].EnableForCurrentBuildTargetGroup();
        }
    }

    public static void PackageNameField()
    {
        EditorGUI.BeginChangeCheck();
        string new_name = EditorGUILayout.TextField("Package Name: ", PlayerSettings.applicationIdentifier);
        if (EditorGUI.EndChangeCheck())
        {
            PlayerSettings.applicationIdentifier = new_name;
        }
    }

    public static void ProductNameField()
    {
        EditorGUI.BeginChangeCheck();
        string new_name = EditorGUILayout.TextField("Product Name: ", PlayerSettings.productName);
        if (EditorGUI.EndChangeCheck())
        {
            PlayerSettings.productName = new_name;
        }
    }

    [MenuItem("Window/XR Test Configuration", priority = 2)]
    static void OpenTestWindow()
    {
        GetWindow<XRTestConfigurationWindow>().Show();
    }

    public int callbackOrder { get { return 0; } }

    public static void RepaintIfOpen()
    {
        if(self != null)
        {
            self.Repaint();
        }
    }
}
