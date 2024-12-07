using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build Game")]
    public static void BuildGame()
    {
        // Get the version from Player Settings
        string version = Application.version;

        // Set the build path and name
        string buildPath = $"SCP_Site_47_v{version}/SCP_Site_47_v{version}.exe";

        // Convert EditorBuildSettingsScene to an array of scene paths
        string[] scenes = GetScenePaths();

        // Build the game
        BuildPipeline.BuildPlayer(
            scenes,
            buildPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );

        Debug.Log($"Build completed: {buildPath}");
    }

    private static string[] GetScenePaths()
    {
        // Filter out enabled scenes and get their paths
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled) // Include only enabled scenes
            .Select(scene => scene.path) // Get the path of each scene
            .ToArray(); // Convert to array
    }
}
