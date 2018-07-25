using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class XRTestSuiteConfiguration
{
    public BuildTarget buildTarget;
    public bool stereoRendering;
    public StereoRenderingPath stereoRenderingPath;
    public RenderingPath renderPath;
    public XRSdkDescription xrSdk;

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(buildTarget);
        sb.Append(xrSdk);

        if (stereoRendering)
        {
            sb.Append("_Stereo");
        }

        if (stereoRenderingPath == StereoRenderingPath.SinglePass)
        {
            sb.Append("_SinglePass");
        }
        if (stereoRenderingPath == StereoRenderingPath.MultiPass)
        {
            sb.Append("_Multipass");
        }
        if (stereoRenderingPath == StereoRenderingPath.Instancing)
        {
            sb.Append("_Instancing");
        }

        sb.Append(renderPath);

        return sb.ToString();
    }

    public bool ShouldBuildTestScene(string scene)
    {
        var scenesToSkip = ScenesToSkipByPlatform().Concat(ScenesToSkipBySdk());
        return !scenesToSkip.Contains(scene);
    }

    IEnumerable<string> ScenesToSkipByPlatform()
    {
        // This function is a stub for now.  
        // If it becomes useful again, use an if statement like this:
        //
        //if (buildTarget == BuildTarget.Android)
        //{
        //    return new[]
        //    {
        //        "Template"
        //    };
        //}

        return Enumerable.Empty<string>();
    }

    IEnumerable<string> ScenesToSkipBySdk()
    {
        // This function is a stub for now.  
        // If it becomes useful again, use an if statement like this:
        //
        //if (xrSdk != XRSdkDescription.s_Oculus)
        //{
        //    return new[] {"VRFocus"};
        //}

        return new[]
        {
            "Assets/Tests/Template/Template.unity"
        };
    }
}
