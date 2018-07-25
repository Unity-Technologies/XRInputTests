using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

[InitializeOnLoad]
public class BuildOverride : MonoBehaviour {

    public static bool override_build = true;

    public static void SetOverride(bool state)
    {
        override_build = state;
        if(override_build)
        {
            Action<BuildPlayerOptions> build_action = Build;
            BuildPlayerWindow.RegisterBuildPlayerHandler(build_action);
        }
        else
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(null);
        }
    }


    static BuildOverride()
    {
        SetOverride(true);
    }
    
    public static void Build(BuildPlayerOptions options)
    {
        if (!BuildPipeline.isBuildingPlayer)
        {
            BuildTarget target = options.target;// EditorUserBuildSettings.activeBuildTarget;
            bool is_mobile = !target.ToString().Contains("Standalone");
            
            if (options.locationPathName != "")
            {
                List<string> scenes = new List<string>();
                foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                {
                    string scene_name = Path.GetFileNameWithoutExtension(scene.path);
                    XRTestsWindow.TestEntryUI entry = XRTestsWindow.GetEntryByName(scene_name);
                    if (entry != null)      //Scene in build settings was also in the scene property database
                    {
                        //Check the entry for build target exclusion, skip if excluded.
                        if (is_mobile && entry.build_target == XRTestsWindow.TestEntryUI.BuildTarget.Standalone)
                        {
                            Debug.Log("Skipping " + scene_name + " for mobile.");
                            continue;
                        }
                        else if (!is_mobile && entry.build_target == XRTestsWindow.TestEntryUI.BuildTarget.Mobile)
                        {
                            Debug.Log("Skipping " + scene_name + " for standalone.");
                            continue;
                        }
                        scenes.Add(scene.path);
                    }
                    else
                    {
                        scenes.Add(scene.path);
                    }
                }

                //Output final build list
                string scene_list = "";
                scenes.ForEach(s => scene_list = (scene_list + s + "\n"));
                Debug.Log("Building " + scenes.Count + " scenes to " + target + " at " + options.locationPathName + "\n" + scene_list);

                options.scenes = scenes.ToArray();

                BuildPipeline.BuildPlayer(options);
            }

        }
        else
        {
            Debug.LogWarning("Unity is already building.");
        }
    }
}
