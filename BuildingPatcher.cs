using System;
using System.Reflection;
using HarmonyLib;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Tools.Builder;

namespace Dropper
{
    [HarmonyPatch(typeof(BuilderTool), "Rotate")]
    public class BuildingPatcher
    {
        private static readonly Logger _logger = new Logger<BuildingPatcher>();

        private static Building GetGhost(BuilderTool toolInstance)
        {
            var toolAccessor = Traverse.Create(toolInstance);
            return (Building) toolAccessor.Property("Ghost").GetValue();
        }

        private static void SetGhost(BuilderTool toolInstance, Building newGhost)
        {
            var toolAccessor = Traverse.Create(toolInstance);
            toolAccessor.Property("Ghost").SetValue(newGhost);
        }
        
        static bool Prefix(BuilderTool __instance)
        {
            _logger.Log($"In Prefix for {__instance}");
            var instanceAccessor = Traverse.Create(__instance);

            try
            {
                var ghost = instanceAccessor.Property("Ghost").GetValue<Building>();
                _logger.Log($"Ghost: {ghost}");
                if (ghost is null) return false;
                var rotationAccessor = instanceAccessor.Field("_rotation");
                var rotation = (BuildingRotation) rotationAccessor.GetValue();
                rotation = rotation.Add(BuildingRotation.Rotate90);
                rotationAccessor.SetValue(rotation);
                _logger.Log($"Set rotation to {rotation}");
                ghost.SetRotation(rotation);
                instanceAccessor.Property("Ghost").SetValue(ghost);
                _logger.Log("Set ghost rotation");
            }
            catch (AmbiguousMatchException)
            {
                return true;
            }

            return false;
        }
    }
}