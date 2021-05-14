using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Game;
using VoxelTycoon.Modding;
using VoxelTycoon.Tools.Builder;
using VoxelTycoon.UI;
using Logger = VoxelTycoon.Logger;

namespace Dropper
{
    // Subclass VoxelTycoon.Modding.Mod as the integration point with the game
    public class DropperMod : Mod
    {
        // Set the hotkey for activating the dropper
        private readonly Hotkey _dropperHotkey = new Hotkey(KeyCode.F2);

        // Set up a logger that outputs nicely
        Logger _logger = new Logger<DropperMod>();

        protected override void Initialize()
        {
            // Initialize is called when your mod .dll is loaded

            _logger.Log("Initialized!");
        }


        protected override void OnGameStarted()
        {
            // OnGameStarted is called when a world is loaded with your mod turned on

            Debug.Log("Dropper Mod is here!");
        }

        protected override void OnUpdate()
        {
            // OnUpdate is called on every game update

            // If user is pressing our key
            if (LazyManager<InputManager>.Current.GetKeyDown(_dropperHotkey))
            {
                // Inspect the state of the UI to get the building under the cursor
                var building = GameUI.Current.BuildingUnderCursor;
                Debug.Log(building);
                if (building == null)
                    return;

                // If the building belongs to the player's company
                if (building.Company == Company.Current)
                {
                    // Fetch the building recipe by its asset_id
                    BuildingRecipe buildingRecipe = BuildingRecipeManager.Current.Get(building.AssetId);
                    _logger.Log("Selected: " + building);

                    // Get the BuilderTool to begin placing a copy of the selected building 
                    BuilderTool builderTool = BuilderToolManager.Current.GetTool(buildingRecipe);
                    _logger.Log("Tool: " + builderTool);

                    // Set the tool as the currently-selected tool
                    UIManager.Current.SetTool(builderTool);
                    _logger.Log("Tool activated");
                }
            }
        }
    }
}