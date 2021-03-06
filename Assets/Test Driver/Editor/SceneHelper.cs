using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneHelper : AssetPostprocessor
{
    public static void OpenTestScene(string scenePath)
    {
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    }

    public static string[] GetTestScenePaths()
    {
        var guids = AssetDatabase.FindAssets("t:scene", new[] {"Assets/Tests"});
        return guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
    }

    public static void CreateNewTestScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.skybox = null;

        var anchor = new GameObject("Camera Anchor");
        anchor.tag = "CameraAnchor";

        var instructions = new GameObject("Instructions");
        instructions.AddComponent<TestInstructions>();
    }
}
