using System;
using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Game;
using VoxelTycoon.Modding;
using VoxelTycoon.Tools.Builder;
using VoxelTycoon.Tools.TrackBuilder;
using VoxelTycoon.Tracks.Rails;
using VoxelTycoon.Tracks.Roads;
using VoxelTycoon.UI;
using Logger = VoxelTycoon.Logger;

namespace Dropper
{
    // Subclass VoxelTycoon.Modding.Mod as the integration point with the game
    public class DropperMod : Mod
    {
        // Set the hotkey for activating the dropper
        private readonly Hotkey _dropperHotkey = new Hotkey(KeyCode.F2);
        private readonly Hotkey _qHotkey = new Hotkey(KeyCode.F4);
        private bool _qShown = false;
        private readonly Hotkey _debugHotkey = new Hotkey(KeyCode.F2, KeyModifier.Control);

        // Set up a logger that outputs nicely
        Logger _logger = new Logger<DropperMod>();

        protected override void Initialize()
        {
            // Initialize is called when your mod .dll is loaded

            _logger.Log("Initialized!");
            UIManager.Current.Root.gameObject.AddComponent<DebugMod>();
        }


        protected override void OnGameStarted()
        {
            // OnGameStarted is called when a world is loaded with your mod turned on

            _logger.Log("Dropper Mod is here!");
        }

        protected void CancelCurrentTool()
        {
            // Taken from VoxelTycoon.UI.UIManager.UpdateTool
            var uiManager = UIManager.Current;
            uiManager.SetTool((ITool) null, true);
        }

        protected void ShowToolQuery()
        {
            GUIHelper.Draw((Action) (() =>
            {
                var currentTool = UIManager.Current.Tool;
                GUILayout.Space(50f);
                GUILayout.TextArea(string.Format("Current Tool: {0}\n", (object) currentTool, (GUILayoutOption[]) Array.Empty<GUILayoutOption>()));
            }));
        }

        protected override void OnUpdate()
        {
            // OnUpdate is called on every game update

            if (LazyManager<InputManager>.Current.GetKeyDown(_debugHotkey))
            {
                DebugMod.showDebug = !DebugMod.showDebug;
                _logger.Log("Toggled debug: " + DebugMod.showDebug);
            }

            // Tool query window (useful for dev/debugging)
            if (LazyManager<InputManager>.Current.GetKeyDown(_qHotkey))
                _qShown = !_qShown;

            if (_qShown)
                ShowToolQuery();
                
            // If user is pressing our key
            if (LazyManager<InputManager>.Current.GetKeyDown(_dropperHotkey))
            {
                // Cancel current tool, if one is in use
                CancelCurrentTool();
                
                // Inspect the state of the UI to get the building under the cursor
                var building = GameUI.Current.BuildingUnderCursor;
                // TODO: Maybe GetGameObjectUnderPointer?

                if (building == null)
                    return;
                _logger.Log(building.ToString());

                // If the building belongs to the player's company
                if (building.Company == Company.Current)
                {
                    var builderTool = GetTool(building);
                    // Get the BuilderTool to begin placing a copy of the selected building 
                    // BuilderTool builderTool = BuilderToolManager.Current.GetTool(buildingRecipe);
                    _logger.Log("Tool: " + builderTool);

                    // Set the tool as the currently-selected tool
                    UIManager.Current.SetTool(builderTool);
                    _logger.Log("Tool activated");
                }
            }
        }

        private ITool GetTool(Building building)
        {
            // Get the appropriate build tool for the selected building
            var buildingType = building.GetType();
            if (buildingType == typeof(RailStation))
            {
                _logger.Log("Returning Station");
                return new RailStationBuilderTool((RailStation) building);
            }
            if (buildingType == typeof(Road))
            {
                _logger.Log("Returning RoadBuilderTool");
                return new RoadBuilderTool((Road) building);
            }

            if (buildingType == typeof(Rail))
            {
                _logger.Log("Returning RailBuilderTool");
                return new RailBuilderTool((Rail) building);
            }
            
            // Fetch the building recipe by its asset_id
            BuildingRecipe buildingRecipe = BuildingRecipeManager.Current.Get(building.AssetId);
            BuilderTool builderTool = BuilderToolManager.Current.GetTool(buildingRecipe);
            return builderTool;
        }
    }
}