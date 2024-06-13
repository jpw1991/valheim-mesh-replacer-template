using System;
using System.IO;
using BepInEx;
using Jotunn;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace ValheimMeshReplacerTemplate
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    internal class ValheimMeshReplacerTemplate : BaseUnityPlugin
    {
        public const string PluginGuid = "com.chebgonaz.valheimmeshreplacertemplate";
        public const string PluginName = "ValheimMeshReplacerTemplate";
        public const string PluginVersion = "0.0.1";

        private string _vanillaPrefab = "SledgeDemolisher";
        private string _bundleName = "mybundle";
        private string _meshName = "mymesh";
        private string _materialName = "mymaterial";
        
        private Mesh _replacementMesh;
        private Material _replacementMaterial;

        private void Awake()
        {
            // First thing we do once the mod has loaded is load our asset bundle.
            LoadAssetBundle();

            // Next, once the vanilla prefabs are available, we do the swap. 
            PrefabManager.OnVanillaPrefabsAvailable += DoOnVanillaPrefabsAvailable;
        }

        private void DoOnVanillaPrefabsAvailable()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= DoOnVanillaPrefabsAvailable;

            // Load the vanilla prefab and log an error if it fails.
            var vanillaPrefab = PrefabManager.Instance.GetPrefab(_vanillaPrefab);
            if (vanillaPrefab == null)
            {
                Logger.LogError($"Failed to get {_vanillaPrefab} prefab");
                return;
            }

            // Get the MeshFilter component from the vanilla prefab. This is holds the mesh. Log an error if it fails.
            var meshFilter = vanillaPrefab.GetComponentInChildren<MeshFilter>(true);
            if (meshFilter == null)
            {
                Logger.LogError($"Failed to get {_vanillaPrefab} prefab's mesh filter");
                return;
            }
            
            meshFilter.mesh = _replacementMesh; // Replace the mesh

            // Now do the same for the material.
            var meshRenderer = vanillaPrefab.GetComponentInChildren<MeshRenderer>(true);
            if (meshRenderer == null)
            {
                Logger.LogError($"Failed to get {_vanillaPrefab} prefab's mesh renderer");
                return;
            }
            
            meshRenderer.sharedMaterial = _replacementMaterial; // Replace the material
        }

        private void LoadAssetBundle()
        {
            var assetBundlePath = Path.Combine(Path.GetDirectoryName(Info.Location), _bundleName);
            var assetBundle = AssetUtils.LoadAssetBundle(assetBundlePath);
            try
            {
                _replacementMesh = assetBundle.LoadAsset<Mesh>(_meshName);
                _replacementMaterial = assetBundle.LoadAsset<Material>(_materialName);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Exception caught while loading assets: {ex}");
            }
            finally
            {
                assetBundle.Unload(false);
            }
        }
    }
}