using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Game;
using VoxelTycoon.Modding;
using VoxelTycoon.Researches;
using VoxelTycoon.Tools.Builder;
using VoxelTycoon.Tools.TrackBuilder;
using VoxelTycoon.Tracks.Conveyors;
using VoxelTycoon.Tracks.Pipes;
using VoxelTycoon.Tracks.Rails;
using VoxelTycoon.Tracks.Roads;
using VoxelTycoon.UI;
using Logger = VoxelTycoon.Logger;
using Object = UnityEngine.Object;

namespace Dropper
{
    // Subclass VoxelTycoon.Modding.Mod as the integration point with the game
    public class DropperMod : Mod
    {
        private static bool _qShown;

        // Set the hotkey for activating the dropper
        private readonly Hotkey _dropperHotkey = new(KeyCode.F2);

        // Set up a logger that outputs nicely
        private readonly Logger _logger = new Logger<DropperMod>();
        private readonly Hotkey _qHotkey = new(KeyCode.F4);

        protected override void Initialize()
        {
            // Initialize is called when your mod .dll is loaded
            var harmony = new Harmony("com.vtdropper.patch");
            harmony.PatchAll();
            _logger.Log("Initialized!");
        }


        protected override void OnGameStarted()
        {
            // OnGameStarted is called when a world is loaded with your mod turned on
            _logger.Log("Dropper Mod is here!");
        }

        private static void CancelCurrentTool()
        {
            // Taken from VoxelTycoon.UI.UIManager.UpdateTool
            var uiManager = UIManager.Current;
            uiManager.SetTool(null, true);
        }

        private static void ShowToolQuery()
        {
            GUIHelper.Draw(() =>
            {
                var currentTool = UIManager.Current.Tool;
                GUILayout.Space(50f);
                GUILayout.TextArea($"Current Tool: {currentTool}\n");
            });
        }

        protected override void OnUpdate()
        {
            // OnUpdate is called on every game update

            // Toggle Tool query window (useful for dev/debugging)
            if (LazyManager<InputManager>.Current.GetKeyDown(_qHotkey))
                _qShown = !_qShown;

            if (_qShown)
                ShowToolQuery();

            // Return if user is not pressing the "dropper" key
            if (!LazyManager<InputManager>.Current.GetKeyDown(_dropperHotkey)) return;

            // Cancel current tool, if one is in use
            CancelCurrentTool();

            // Inspect the state of the UI to get the building under the cursor
            var building = GameUI.Current.BuildingUnderCursor;
            // TODO: Maybe GetGameObjectUnderPointer?

            if (building == null)
                return;
            _logger.Log($"Selected building: {building}");

            // Return if the building doesn't belong to the player's company
            if (building.Company != Company.Current) return;

            // Get the BuilderTool to begin placing a copy of the selected building 
            var builderTool = GetTool(building);

            // Set the tool active in the UI
            UIManager.Current.SetTool(builderTool);

            // Create accessor for protected/private members
            var toolAccessor = Traverse.Create(builderTool);

            // Set rotation of copy same as original building
            if (builderTool is BuilderTool tool)
            {
                tool.Rotation = building.Rotation;
            }

            switch (building)
            {
                case Warehouse warehouse:
                {
                    var ghost = (StorageNetworkBuilding) toolAccessor.Property("Ghost").GetValue();
                    if (warehouse.Storage != null)
                    {
                        _logger.Log("Copying storage");
                        if (ghost != null)
                        {
                            ghost.Storage = warehouse.Storage.Instantiate(false);
                            toolAccessor.Property("Ghost").SetValue(ghost);
                        }
                        // ((Warehouse) ((StorageNetworkBuildingBuilderTool) builderTool).Recipe.Building).Storage =
                        //     warehouse.Storage.Instantiate(false);
                    }
                    else
                    {
                        _logger.Log("Null storage?");
                    }
            
                    break;
                }
                case Device device:
                {
                    var ghost = (Device) toolAccessor.Property("Ghost").GetValue();
                    if (ghost != null)
                    {
                        Traverse.Create(ghost).Field("_recipe").SetValue(device.Recipe);
                        toolAccessor.Property("Ghost").SetValue(ghost);
                    }

                    break;
                }
            }

            _logger.Log("Tool activated");
        }

        private ITool GetTool(Building building)
        {
            // Map of "special" types of buildings that need specific tools
            var toolMapper = new Dictionary<Type, Func<Building, ITool>>
            {
                {typeof(RailStation), b => new RailStationBuilderTool((RailStation) b)},
                {typeof(Road), b => new RoadBuilderTool((Road) b)},
                {typeof(Rail), b => new RailBuilderTool((Rail) b)},
                {typeof(Pipe), b => new PipeBuilderTool((Pipe) b)},
                {typeof(Conveyor), b => new ConveyorBuilderTool((Conveyor) b)}
            };

            // Get the appropriate build tool for the selected building if it's one of the "special" types
            var buildingType = building.GetType();
            if (toolMapper.TryGetValue(buildingType, out var toolFactory))
            {
                _logger.Log("Returning mapped type " + toolFactory);
                return toolFactory(building);
            }

            // Otherwise, fetch the building recipe by its asset_id
            var buildingRecipe = BuildingRecipeManager.Current.Get(building.AssetId);
            buildingRecipe.Building = UnityEngine.Object.Instantiate<Building>(building);

            var builderTool = BuilderToolManager.Current.GetTool(buildingRecipe);
            
            // Get current tool Recipe
            _logger.Log($"Recipe: {builderTool.Recipe}");
            
            // Create a copy of the current building to replicate settings
            var newBuilding = UnityEngine.Object.Instantiate<Building>(building);
            
            // Set storage if it's a warehouse
            if (building is Warehouse warehouse)
            {
                Warehouse newWarehouse = (Warehouse) UnityEngine.Object.Instantiate<Warehouse>(warehouse);
                newBuilding = newWarehouse;
                _logger.Log($"Old storage: {warehouse.Storage} New: {newWarehouse.Storage}");
                if (warehouse.Storage != null)
                    newWarehouse.Storage = warehouse.Storage.Instantiate(false);
            }

            // Set recipe if it's a factory
            if (building is Device device)
            {
                _logger.Log($"Old recipe: {device.Recipe} New: {((Device) newBuilding).Recipe}");
                Traverse.Create(newBuilding).Field("_recipe").SetValue(device.Recipe);
            }

            // TODO: Copy research to new Lab
            
            _logger.Log("New building: " + newBuilding);
            
            // Set recipe building to one with duplicated settings
            builderTool.Recipe.Building = newBuilding;

            return builderTool;
        }
    }
}