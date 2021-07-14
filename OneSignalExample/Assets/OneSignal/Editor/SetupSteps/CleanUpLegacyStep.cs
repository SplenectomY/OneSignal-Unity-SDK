using System.IO;
using System.Linq;
using UnityEditor;

/// <summary>
/// Handles if there are files within the Assets/OneSignal folder which should not be there. Typically this
/// indicates the presence of legacy files.
/// </summary>
public sealed class CleanUpLegacyStep : OneSignalSetupStep
{
    public override string Summary
        => "Remove legacy files";

    public override string Details
        => "Checks for the diff between the files distributed with the package and those which are in the " +
           OneSignalFileInventory.PackageAssetsPath;

    public override string DocumentationLink
        => "";

    protected override bool _getIsStepCompleted()
    {
        if (_inventory == null)
            _inventory = AssetDatabase.LoadAssetAtPath<OneSignalFileInventory>(OneSignalFileInventory.AssetPath);

        if (_inventory == null)
            return true;

        var currentPaths = _inventory.GetCurrentPaths();
        var diff = currentPaths.Except(_inventory.DistributedPaths);
        return !diff.Any();
    }

    protected override void _runStep()
    {
        if (_inventory == null)
            _inventory = AssetDatabase.LoadAssetAtPath<OneSignalFileInventory>(OneSignalFileInventory.AssetPath);

        if (_inventory == null)
            return; // error
        
        var currentPaths = _inventory.GetCurrentPaths();
        var diff = currentPaths.Except(_inventory.DistributedPaths);

        foreach (var path in diff)
            File.Delete(path);
    }

    private OneSignalFileInventory _inventory;
}