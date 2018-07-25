using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class XRTestsWindow : EditorWindow, UnityEditor.Build.IActiveBuildTargetChanged
{
    Vector2 m_Scroll;
    List<TestEntryUI> m_Tests = new List<TestEntryUI>();

    private readonly static string m_RelativePath = "/Resources/SceneBuildProperties.txt";
    private static string m_DataPath { get { return Application.dataPath + m_RelativePath; } }
    private static XRTestsWindow m_Self;

    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        Repaint();
    }
    
    public int callbackOrder { get { return 0; } }

    public static void OnSceneAssetChanged()
    {
        var testWindow = GetWindow<XRTestsWindow>();
        if (testWindow)
            testWindow.PopulateTestEntries();
    }

    void PopulateTestEntries()
    {
        m_Tests.Clear();
        var testScenePaths = SceneHelper.GetTestScenePaths();
        for (var testSceneIndex = 0; testSceneIndex < testScenePaths.Length; testSceneIndex++)
        {
            var scenePath = testScenePaths[testSceneIndex];
            var testEntryUi = new TestEntryUI(scenePath, testSceneIndex);
            m_Tests.Add(testEntryUi);
        }

        //Always load from file before writing when recomputing the database
        UpdateFromFile(m_DataPath);
        WriteToFile(m_DataPath);

        Repaint();
    }
    
    void OnEnable()
    {
        titleContent = new GUIContent("XR Tests");
        m_Self = this;
        PopulateTestEntries();
    }

    void OnGUI()
    {
        //bool build = false;
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            if (ToolbarButton("New", "Create a new test scene."))
            {
                SceneHelper.CreateNewTestScene();
                return;
            }

            if (ToolbarButton("Refresh", "Refresh the list of tests, in case tests that were added/removed are not reflected in the list."))
                PopulateTestEntries();

            GUILayout.FlexibleSpace();

            bool override_toggle = ToolbarToggle(BuildOverride.override_build, "Override Scene List", "True to override the scenes in the Build Setting's scene list with the settings below.");
            if(override_toggle != BuildOverride.override_build)
            {
                BuildOverride.SetOverride(override_toggle);
            }
        }
        

        DrawTestList();
    }

    public static TestEntryUI GetEntryByName(string name)
    {
        if (m_Self != null)
        {
            foreach (TestEntryUI entry in m_Self.m_Tests)
            {
                if (entry.m_TestName == name)
                {
                    return entry;
                }
            }
        }
        return null;
    }
    
    static bool ToolbarButton(string title, string tooltip)
    {
        return GUILayout.Button(new GUIContent(title, tooltip), EditorStyles.toolbarButton);
    }
    
    static bool ToolbarToggle(bool value, string title, string tooltip)
    {
        return GUILayout.Toggle(value, new GUIContent(title, tooltip), EditorStyles.toolbarButton);
    }

    void DrawTestList()
    {
        using (var scroll = new GUILayout.ScrollViewScope(m_Scroll, false, false))
        {
            m_Scroll = scroll.scrollPosition;

            foreach (var testEntryUi in m_Tests)
                testEntryUi.Draw();
        }
    }

    [MenuItem("Window/XR Tests", priority = 1)]
    static void OpenTestWindow()
    {
        GetWindow<XRTestsWindow>().Show();
    }

    //Writes the current contents to file.  NOTE: Does not update the contents from file first.
    public static void OverwriteDatabase()
    {
        m_Self.WriteToFile(m_DataPath);
    }

    //Writes current test list to scene property database file
    private void WriteToFile(string path)
    {
        if (File.Exists(m_DataPath))
        {
            File.Delete(m_DataPath);
        }

        StreamWriter writer = new StreamWriter(path, false, Encoding.ASCII);
        
        foreach (TestEntryUI entry in m_Tests)
        {
            entry.WriteLine(writer);
        }
        
        writer.Close();
        
        UnityEditor.AssetDatabase.Refresh();
    }

    //Updates existing entries based on previous database file contents
    private void UpdateFromFile(string path)
    {
        if (!File.Exists(m_DataPath))
        {
            return;
        }

        //Load database entries into a new list
        List<TestEntryUI> old_entries = new List<TestEntryUI>();
        string line;
        StreamReader reader = new StreamReader(path, Encoding.ASCII);

        do
        {
            line = reader.ReadLine();
            if (line != null)
            {
                old_entries.Add(new TestEntryUI(line));
            }

        } while (line != null);

        reader.Close();

        //Compare existing contents with database file.  Overwrite properties (only) in new database file with old database file entries.  Delete old database entires
        foreach(TestEntryUI entry in m_Tests)
        {
            foreach(TestEntryUI old_entry in old_entries)
            {
                if(old_entry.m_TestName == entry.m_TestName)
                {
                    entry.CopyProperties(old_entry);
                }
            }
        }
    }
    
    public class TestEntryUI
    {
        string m_ScenePath;
        int m_TestSceneIndex;
        public string m_TestName { get; private set; }
        public BuildTarget build_target { get; private set; }

        public enum BuildTarget : int { Both = 0, Standalone = 1, Mobile = 2 }

        //Constructs from a tagged db string.  These are generally created during UpdateFromFile as temporary items for comparison with the current scene list contents.
        public TestEntryUI(string db_string)
        {
            Func<string, string, string> get_string_by_tag = delegate (string full_string, string tag) {
                Match match = Regex.Match(full_string, ("<" + tag + ">(.*?)<\\/" + tag + ">"));
                if (match.Success)
                {
                    return match.Groups[1].ToString();
                }
                else
                {
                    return "";
                }
            };

            m_TestName = get_string_by_tag(db_string, "name");

            //Convert to a build target Enum
            build_target = BuildTarget.Both;
            var targets = Enum.GetValues(typeof(BuildTarget)).Cast<BuildTarget>().ToArray<BuildTarget>();
            int build_target_value = Convert.ToInt32(get_string_by_tag(db_string, "target"));
            foreach (BuildTarget t in targets)
            {
                if( (int)t == build_target_value)
                {
                    build_target = t;
                }
            }

        }

        //Copies parameters from a different source.
        public void CopyProperties(TestEntryUI source)
        {
            build_target = source.build_target;
        }

        //Writes a line to the scene properties database file
        public void WriteLine(StreamWriter writer)
        {
            writer.WriteLine(("<name>" + m_TestName + "</name><target>" + (int)build_target + "</target>"));
        }

        public TestEntryUI(string scenePath, int index)
        {
            m_ScenePath = scenePath;

            // Test scene indices start at 1, not 0.
            m_TestSceneIndex = index + 1;
            m_TestName = Path.GetFileNameWithoutExtension(m_ScenePath);
        }

        public void Draw()
        {
            using (new GUILayout.HorizontalScope("box"))
            {
                DrawLabel();
                DrawBuildSettings();
                DrawButtons();
            }
        }

        void DrawButtons()
        {
            GUI.color = Color.white;

            if (TestButton("Run", string.Format("Run {0} in the editor.", m_TestName)))
                RunTest();

            using (new GuiEnabledScope(!Application.isPlaying))
            {
                if (TestButton("Edit", string.Format("Open {0} and the start scene in the editor.", m_TestName)))
                    SceneHelper.OpenTestScene(m_ScenePath);
                if (TestButton("Export", string.Format("Export {0} to a new Unity project.", m_TestName)))
                    XRTestExportWindow.ExportTestScene(m_ScenePath);
            }
        }

        void RunTest()
        {
            if (Application.isPlaying)
            {
                FindObjectOfType<TestDriver>().RunTest(m_TestSceneIndex);
            }
            else
            {
                SceneHelper.OpenTestScene(m_ScenePath);
                EditorApplication.isPlaying = true;
            }
        }

        void DrawBuildSettings()
        {
            BuildTarget new_target = (BuildTarget)EditorGUILayout.EnumPopup(build_target);
            if(new_target != build_target)
            {
                build_target = new_target;
                XRTestsWindow.OverwriteDatabase();
            }

        }

        void DrawLabel()
        {
            EditorGUILayout.LabelField(m_TestName);
        }

        bool TestButton(string title, string tooltip)
        {
            var layout = GUILayout.Width(50 * EditorGUIUtility.pixelsPerPoint);
            return GUILayout.Button(new GUIContent(title, tooltip), layout);
        }

        class GuiEnabledScope : IDisposable
        {
            readonly bool m_WasEnabled;

            public GuiEnabledScope(bool shouldEnable)
            {
                m_WasEnabled = GUI.enabled;
                GUI.enabled = m_WasEnabled && shouldEnable;
            }

            public void Dispose()
            {
                GUI.enabled = m_WasEnabled;
            }
        }
    }
}
