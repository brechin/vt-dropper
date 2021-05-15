using System;
using UnityEngine;
using VoxelTycoon;

namespace Dropper
{
    public class DebugMod : MonoBehaviour
    {
        private static Rect _windowRect = new Rect(100, 100, 300, 500);
        static VoxelTycoon.Logger _logger = new Logger<DebugMod>();
        public static bool showDebug = false;
        
        private static Vector2 _scrollPosition = Vector2.zero;

        private static void DrawWindowContents(int windowID)
        {
            GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.BeginVertical("box");
            GUI.enabled = true;
            DebugSettings.IgnoreRegions = AddToggle(DebugSettings.IgnoreRegions, "IgnoreRegions");
            DebugSettings.ShowHiddenItems = AddToggle(DebugSettings.ShowHiddenItems, "ShowHiddenItems");
            DebugSettings.IgnoreDepotMatching = AddToggle(DebugSettings.IgnoreDepotMatching, "IgnoreDepotMatching");
            DebugSettings.CurrentCompany = AddToggle(DebugSettings.CurrentCompany, "CurrentCompany");
            DebugSettings.Counters = AddToggle(DebugSettings.Counters, "Counters");
            DebugSettings.PreviewIcons = AddToggle(DebugSettings.PreviewIcons, "PreviewIcons");
            DebugSettings.SaveIconsToDisk = AddToggle(DebugSettings.SaveIconsToDisk, "SaveIconsToDisk");

            DebugSettings.ForceLinearVolume = AddToggle(DebugSettings.ForceLinearVolume, "ForceLinearVolume");
            DebugSettings.CameraFloat = AddToggle(DebugSettings.CameraFloat, "CameraFloat");
            DebugSettings.SwitchCameraViewsRandomly =
                AddToggle(DebugSettings.SwitchCameraViewsRandomly, "SwitchCameraViewsRandomly");
            DebugSettings.Inflation = AddToggle(DebugSettings.Inflation, "Inflation");
            DebugSettings.ResearchRequirements = AddToggle(DebugSettings.ResearchRequirements, "ResearchRequirements");
            DebugSettings.ForceWhiteResearchCategory =
                AddToggle(DebugSettings.ForceWhiteResearchCategory, "ForceWhiteResearchCategory");
            DebugSettings.Coordinates = AddToggle(DebugSettings.Coordinates, "Coordinates");
            DebugSettings.OutputInfinite = AddToggle(DebugSettings.OutputInfinite, "OutputInfinite");
            DebugSettings.ConsumeInfinite = AddToggle(DebugSettings.ConsumeInfinite, "ConsumeInfinite");

            DebugSettings.InsaneMines = AddToggle(DebugSettings.InsaneMines, "InsaneMines");
            DebugSettings.ExtendedInfoToolMode = AddToggle(DebugSettings.ExtendedInfoToolMode, "ExtendedInfoToolMode");
            DebugSettings.AssetEditingHelper = AddToggle(DebugSettings.AssetEditingHelper, "AssetEditingHelper");
            DebugSettings.StorageNetwork = AddToggle(DebugSettings.StorageNetwork, "StorageNetwork");
            DebugSettings.Passengers = AddToggle(DebugSettings.Passengers, "Passengers");
            DebugSettings.DeltaTimeDifference = AddToggle(DebugSettings.DeltaTimeDifference, "DeltaTimeDifference");
            DebugSettings.PoolManager = AddToggle(DebugSettings.PoolManager, "PoolManager");
            DebugSettings.CityGrowth = AddToggle(DebugSettings.CityGrowth, "CityGrowth");
            DebugSettings.UpdateBehaviourPerfomance =
                AddToggle(DebugSettings.UpdateBehaviourPerfomance, "UpdateBehaviourPerfomance");
            DebugSettings.RailBlockManager = AddToggle(DebugSettings.RailBlockManager, "RailBlockManager");
            // [Header("Cities")]
            DebugSettings.LogCityDemandRecipeProbabilities = AddToggle(DebugSettings.LogCityDemandRecipeProbabilities,
                "LogCityDemandRecipeProbabilities");
            DebugSettings.ReactiveCityGrowth = AddToggle(DebugSettings.ReactiveCityGrowth, "ReactiveCityGrowth");
            DebugSettings.ReactiveCityDemands = AddToggle(DebugSettings.ReactiveCityDemands, "ReactiveCityDemands");
            DebugSettings.AllowCityRemoving = AddToggle(DebugSettings.AllowCityRemoving, "AllowCityRemoving");
            DebugSettings.AllowHeadquartersRemoving =
                AddToggle(DebugSettings.AllowHeadquartersRemoving, "AllowHeadquartersRemoving");
            DebugSettings.DemandLimitTooltip = AddToggle(DebugSettings.DemandLimitTooltip, "DemandLimitTooltip");
            DebugSettings.DemandLevelTooltip = AddToggle(DebugSettings.DemandLevelTooltip, "DemandLevelTooltip");
            // [Header("Buildings")]
            DebugSettings.BuildingIds = AddToggle(DebugSettings.BuildingIds, "BuildingIds");
            DebugSettings.BuildingPositions = AddToggle(DebugSettings.BuildingPositions, "BuildingPositions");
            DebugSettings.BuildingBoundingBoxes =
                AddToggle(DebugSettings.BuildingBoundingBoxes, "BuildingBoundingBoxes");
            DebugSettings.BuildingForwards = AddToggle(DebugSettings.BuildingForwards, "BuildingForwards");
            DebugSettings.BuildingParent = AddToggle(DebugSettings.BuildingParent, "BuildingParent");
            DebugSettings.BuildingChildren = AddToggle(DebugSettings.BuildingChildren, "BuildingChildren");
            DebugSettings.BuildingFacades = AddToggle(DebugSettings.BuildingFacades, "BuildingFacades");
            DebugSettings.StorageBuildingSiblings =
                AddToggle(DebugSettings.StorageBuildingSiblings, "StorageBuildingSiblings");
            DebugSettings.RoadSidewalks = AddToggle(DebugSettings.RoadSidewalks, "RoadSidewalks");
            DebugSettings.ConveyorOperatorSpawnConnections = AddToggle(DebugSettings.ConveyorOperatorSpawnConnections,
                "ConveyorOperatorSpawnConnections");
            // [Header("Cargoes")]
            DebugSettings.CargoManager = AddToggle(DebugSettings.CargoManager, "CargoManager");
            DebugSettings.MasterCargoes = AddToggle(DebugSettings.MasterCargoes, "MasterCargoes");
            DebugSettings.MasterCargoesOrder = AddToggle(DebugSettings.MasterCargoesOrder, "MasterCargoesOrder");
            DebugSettings.SlaveCargoes = AddToggle(DebugSettings.SlaveCargoes, "SlaveCargoes");
            DebugSettings.SlaveCargoesOrder = AddToggle(DebugSettings.SlaveCargoesOrder, "SlaveCargoesOrder");
            DebugSettings.ObstacleCargoesOrder = AddToggle(DebugSettings.ObstacleCargoesOrder, "ObstacleCargoesOrder");
            DebugSettings.ConveyorNextConnections =
                AddToggle(DebugSettings.ConveyorNextConnections, "ConveyorNextConnections");
            DebugSettings.ConveyorInputsOutputs =
                AddToggle(DebugSettings.ConveyorInputsOutputs, "ConveyorInputsOutputs");
            // [Header("Tracks")]
            DebugSettings.TrackPool = AddToggle(DebugSettings.TrackPool, "TrackPool");
            DebugSettings.TrackConnections = AddToggle(DebugSettings.TrackConnections, "TrackConnections");
            DebugSettings.TrackUnits = AddToggle(DebugSettings.TrackUnits, "TrackUnits");
            DebugSettings.TrackPaths = AddToggle(DebugSettings.TrackPaths, "TrackPaths");
            DebugSettings.TrackPathsSimplified = AddToggle(DebugSettings.TrackPathsSimplified, "TrackPathsSimplified");
            DebugSettings.TrackJoints = AddToggle(DebugSettings.TrackJoints, "TrackJoints");
            DebugSettings.TrackPathNodeManager = AddToggle(DebugSettings.TrackPathNodeManager, "TrackPathNodeManager");
            // [Header("Rails")]
            DebugSettings.RailBlocks = AddToggle(DebugSettings.RailBlocks, "RailBlocks");
            DebugSettings.RailLinks = AddToggle(DebugSettings.RailLinks, "RailLinks");
            DebugSettings.RailSignalPositions = AddToggle(DebugSettings.RailSignalPositions, "RailSignalPositions");
            DebugSettings.RailSignalValues = AddToggle(DebugSettings.RailSignalValues, "RailSignalValues");
            DebugSettings.RailSignalConnections =
                AddToggle(DebugSettings.RailSignalConnections, "RailSignalConnections");
            DebugSettings.RailSignalExits = AddToggle(DebugSettings.RailSignalExits, "RailSignalExits");
            // [Header("Track Units")]
            DebugSettings.TrackUnitManager = AddToggle(DebugSettings.TrackUnitManager, "TrackUnitManager");
            DebugSettings.TrackUnitPaths = AddToggle(DebugSettings.TrackUnitPaths, "TrackUnitPaths");
            DebugSettings.TrackUnitBounds = AddToggle(DebugSettings.TrackUnitBounds, "TrackUnitBounds");
            // [Header("Vehicles")]
            DebugSettings.VehicleBreakingDistance =
                AddToggle(DebugSettings.VehicleBreakingDistance, "VehicleBreakingDistance");
            DebugSettings.VehicleSlopes = AddToggle(DebugSettings.VehicleSlopes, "VehicleSlopes");
            DebugSettings.VehicleForces = AddToggle(DebugSettings.VehicleForces, "VehicleForces");
            DebugSettings.VehicleUpdatePath = AddToggle(DebugSettings.VehicleUpdatePath, "VehicleUpdatePath");
            DebugSettings.VehicleUnitIndicator = AddToggle(DebugSettings.VehicleUnitIndicator, "VehicleUnitIndicator");
            DebugSettings.VehicleUnitCabs = AddToggle(DebugSettings.VehicleUnitCabs, "VehicleUnitCabs");
            DebugSettings.VehicleDestinationStops =
                AddToggle(DebugSettings.VehicleDestinationStops, "VehicleDestinationStops");
            DebugSettings.VehicleDestinationTarget =
                AddToggle(DebugSettings.VehicleDestinationTarget, "VehicleDestinationTarget");
            DebugSettings.VehicleDepotSpawnConnections = AddToggle(DebugSettings.VehicleDepotSpawnConnections,
                "VehicleDepotSpawnConnections");
            DebugSettings.VehicleStationStopConnections = AddToggle(DebugSettings.VehicleStationStopConnections,
                "VehicleStationStopConnections");
            DebugSettings.VehiclePathNodes = AddToggle(DebugSettings.VehiclePathNodes, "VehiclePathNodes");
            DebugSettings.VehicleVisitedTrackPathNodes = AddToggle(DebugSettings.VehicleVisitedTrackPathNodes,
                "VehicleVisitedTrackPathNodes");
            // [Header("Trains")]
            DebugSettings.TrainNextSignals = AddToggle(DebugSettings.TrainNextSignals, "TrainNextSignals");
            DebugSettings.TrainAudioDebug = AddToggle(DebugSettings.TrainAudioDebug, "TrainAudioDebug");
            DebugSettings.TrainAudioClacks = AddToggle(DebugSettings.TrainAudioClacks, "TrainAudioClacks");
            DebugSettings.TrainDummy = AddToggle(DebugSettings.TrainDummy, "TrainDummy");
            DebugSettings.TrainUnitSqueal = AddToggle(DebugSettings.TrainUnitSqueal, "TrainUnitSqueal");
            DebugSettings.TrainBogies = AddToggle(DebugSettings.TrainBogies, "TrainBogies");
            // [Header("UI")]
            DebugSettings.DisableIndicatorsSort =
                AddToggle(DebugSettings.DisableIndicatorsSort, "DisableIndicatorsSort");
            DebugSettings.SlowDownUIAnimations = AddToggle(DebugSettings.SlowDownUIAnimations, "SlowDownUIAnimations");
            // [Header("Assets")]
            DebugSettings.OverwriteAssets = AddToggle(DebugSettings.OverwriteAssets, "OverwriteAssets");
            DebugSettings.WritePatchedAssets = AddToggle(DebugSettings.WritePatchedAssets, "WritePatchedAssets");
            
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private static bool AddToggle(bool value, string name)
        {
            return GUILayout.Toggle(value, name);
        }

        void OnGUI()
        {
            if (showDebug)
            {
                GUIHelper.Draw((Action) (() =>
                {
                    _windowRect = GUILayout.Window(0, _windowRect, DrawWindowContents, "My Window");
                }));
            }
        }
    }
}