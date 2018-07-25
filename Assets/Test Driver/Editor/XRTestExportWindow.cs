using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRTestExportWindow : EditorWindow
{
    string m_TestScenePath;
    string m_ExportTestScenePath;
    string m_TestName;

    void OnGUI()
    {
        if (GUILayout.Button("Export"))
            ExportTest();
    }

    void ExportTest()
    {
        var setup = EditorSceneManager.GetSceneManagerSetup();

        try
        {
            var testScene = MergeTestAndStartScenes();

            var extraScriptsRequired = new HashSet<string>();
            
            //RemoveTestComponentsWeDontNeed(testScene);
            RemoveCameraAnchor(testScene);
            //HandleTestUi(testScene, extraScriptsRequired);
            EditorSceneManager.SaveScene(testScene);
            ExportTestSceneToNewProject(extraScriptsRequired);
        }
        finally
        {
            try
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
            }
            catch
            {
                Debug.LogError("Failed to restore scene manager setup after export.");
            }

            if (File.Exists(m_ExportTestScenePath))
            {
                File.Delete(m_ExportTestScenePath);
                File.Delete(m_ExportTestScenePath + ".meta");
                AssetDatabase.Refresh();
            }

            Close();
        }

        Debug.Log("Test Export Complete");
    }

    void HandleTestUi(Scene testScene, HashSet<string> extraScriptsRequired)
    {
        foreach (var obj in testScene.GetRootGameObjects())
        {
            if (obj.name.Contains("Test UI"))
            {
                DestroyImmediate(obj);
                continue;
            }

            var instructions = obj.GetComponentInChildren<TestInstructions>();
            if (instructions)
            {
                DestroyImmediate(instructions);
                DestroyObjectIfEmpty(obj);
            }
        }
    }

    Scene MergeTestAndStartScenes()
    {
        var testScene = EditorSceneManager.OpenScene(m_TestScenePath, OpenSceneMode.Single);
        var startScene = EditorSceneManager.OpenScene(SceneHelper.StartScene, OpenSceneMode.Additive);
        SceneManager.MergeScenes(startScene, testScene);
        EditorSceneManager.SaveScene(testScene, m_ExportTestScenePath, false);
        return testScene;
    }

    void RemoveCameraAnchor(Scene testScene)
    {
        foreach (var obj in testScene.GetRootGameObjects())
        {
            if (obj.CompareTag("CameraAnchor"))
            {
                Camera.main.transform.position = obj.transform.position;

                DestroyImmediate(obj);
            }
        }
    }

    static void RemoveTestComponentsWeDontNeed(Scene testScene)
    {
        var componentTypesToRemove = new[]
        {
            typeof(TestDriver)
        };
        foreach (var obj in testScene.GetRootGameObjects())
        {
            foreach (var type in componentTypesToRemove)
            {
                var components = obj.GetComponentsInChildren(type);
                foreach (var c in components)
                    DestroyImmediate(c);
            }
        }
    }

    static void DestroyObjectIfEmpty(GameObject obj)
    {
        var comps = new List<Component>();
        obj.GetComponents(comps);
        if (comps.Count == 1 && obj.transform.childCount == 0)
            DestroyImmediate(obj);
    }

    void ExportTestSceneToNewProject(HashSet<string> extraScriptsRequired)
    {
        var rootPath = Path.GetDirectoryName(Application.dataPath);
        var outputProjectPath = EditorUtility.OpenFolderPanel("Location for New Project", Path.GetDirectoryName(rootPath), "");

        // OpenFolderPanel returns null/empty if cancel is pressed
        if (string.IsNullOrEmpty(outputProjectPath))
            return;

        outputProjectPath = Path.Combine(outputProjectPath, "XRTestSuite-" + m_TestName);

        CreateEmptyProjectDirectory(outputProjectPath);
        CopyProjectSettings(outputProjectPath, rootPath);
        CopyDependencies(rootPath, outputProjectPath, extraScriptsRequired);
    }

    static void CreateEmptyProjectDirectory(string outputProjectPath)
    {
        if (Directory.Exists(outputProjectPath))
        {
            foreach (var file in Directory.GetFiles(outputProjectPath, "*", SearchOption.AllDirectories)) {
                File.Delete(file);
            }
            Directory.Delete(outputProjectPath, true);
        }

        Directory.CreateDirectory(outputProjectPath);
    }

    void CopyDependencies(string rootPath, string outputProjectPath, IEnumerable<string> extraScriptsRequired)
    {
        var sceneDependencies = AssetDatabase.GetDependencies(m_ExportTestScenePath, true);
        var filesToCopy = sceneDependencies.Concat(extraScriptsRequired).Distinct();
        foreach (var file in filesToCopy)
            CopyFile(rootPath, outputProjectPath, file);
        
        CopyFile(rootPath, outputProjectPath, "Assets/Test Driver/InputMobileForSceneChange.cs");
        CopyFile(rootPath, outputProjectPath, "Assets/README.txt");
    }

    static void CopyFile(string rootPath, string outputProjectPath, string file)
    {
        var outputFile = Path.Combine(outputProjectPath, file);
        var sourceFile = Path.Combine(rootPath, file);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

        File.Copy(sourceFile, outputFile);
        File.Copy(sourceFile + ".meta", outputFile + ".meta");
    }

    void CopyProjectSettings(string outputProjectPath, string rootPath)
    {
        // This method changes build settings so our EditorBuildSettings asset just has the one scene,
        // so we need to make sure that no matter what we update our build settings back to the way
        // they should be for the suite.
        try
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(m_ExportTestScenePath, true),
            };
            AssetDatabase.SaveAssets();

            var outputProjectSettings = Path.Combine(outputProjectPath, "ProjectSettings");
            Directory.CreateDirectory(outputProjectSettings);
            foreach (var file in Directory.GetFiles(Path.Combine(rootPath, "ProjectSettings")))
            {
                var destFile = Path.Combine(outputProjectSettings, Path.GetFileName(file));
                File.Copy(file, destFile);
            }
        }
        finally
        {
            XRBuildSettings.UpdateBuildSettings();
        }
    }

    public static void ExportTestScene(string testScenePath)
    {
        var window = GetWindow<XRTestExportWindow>(true, "Export " + Path.GetFileNameWithoutExtension(testScenePath));
        window.m_TestScenePath = testScenePath;
        window.m_TestName = Path.GetFileNameWithoutExtension(testScenePath);
        window.m_ExportTestScenePath = "Assets/" + window.m_TestName + ".unity";
    }
}
