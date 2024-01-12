using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build for Steam")]
    public static void BuildForSteam()
    {
        // Set preprocessor directive for Steam, preserving existing symbols
        SetScriptingDefineSymbols(BuildTargetGroup.Standalone, "STEAM");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            //scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" }, // Add your scenes here
            locationPathName = "Builds/Steam/Game.exe",
            target = BuildTarget.StandaloneWindows64,
            //options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Build for Steam complete");
        Debug.Log(buildPlayerOptions.scenes);
    }

    [MenuItem("Build/Build for Kongregate")]
    public static void BuildForKongregate()
    {
        // Set preprocessor directive for Kongregate, preserving existing symbols
        SetScriptingDefineSymbols(BuildTargetGroup.WebGL, "KONGREGATE");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" }, // Add your scenes here
            locationPathName = "Builds/Kongregate/Game.html",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    private static void SetScriptingDefineSymbols(BuildTargetGroup targetGroup, string newSymbol)
    {
        string existingSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        if (!existingSymbols.Contains(newSymbol))
        {
            existingSymbols += (string.IsNullOrEmpty(existingSymbols) ? "" : ";") + newSymbol;
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, existingSymbols);
    }
}
