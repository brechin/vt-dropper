using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Game;
using VoxelTycoon.Modding;

namespace Dropper
{
    public class DropperMod : Mod
    {
        private Hotkey _dropperHotkey = new Hotkey(KeyCode.Tilde);
        
        protected override void OnGameStarted()
        {
            
        }

        protected override void OnUpdate()
        {
            if (LazyManager<InputManager>.Current.GetKeyDown(_dropperHotkey))
            {
                var building = GameUI.Current.BuildingUnderCursor;
                Debug.Log(building);
                Debug.Log(building.IsBuilt);
            }
            
        }
    }
}