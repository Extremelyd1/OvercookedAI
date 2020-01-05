using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AI {
    internal static class Debug {

        public static void LogTestReflectionUtil() {
            Logger.Log("Test1");
            ClientWorkstation[] stations = Object.FindObjectsOfType<ClientWorkstation>();
            Logger.Log("Test2");
            foreach (ClientWorkstation station in stations) {
                Logger.Log("Test3");
                // ReflectionUtil.GetValue(station, "m_attachStation");
                ClientAttachStation attachStation =
                    (ClientAttachStation) ReflectionUtil.GetValue(station, "m_attachStation");
                Logger.Log($"Test4: {attachStation.name}");
                if (attachStation.InspectItem() != null) {
                    Logger.Log($"Item on workstation: {attachStation.InspectItem()}");
                } else {
                    Logger.Log($"No item on workstation");
                }

                Logger.Log("Test5");
            }
            Logger.Log("Test6");
        }
        
        public static void LogInteractables() {
            PlayerControls[] playerControlsArray = Object.FindObjectsOfType<PlayerControls>();

            PlayerControls bot = playerControlsArray[1];
                
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.Static;
            MethodInfo findNearbyMethod = playerControlsArray[1].GetType().GetMethod("FindNearbyObjects", bindFlags);
                
            PlayerControls.InteractionObjects interactionObjects = (PlayerControls.InteractionObjects) 
                findNearbyMethod.Invoke(bot, new object[] {});

            if (interactionObjects.m_gridLocation != null) {
                Vector3 position = interactionObjects.m_gridLocation.AccessGridManager.GetPosFromGridLocation(
                    interactionObjects
                        .m_gridLocation.GridIndex);

                Logger.Log($"interactionObject location: {Logger.FormatPosition(position)}");
            }

            if (interactionObjects.m_interactable != null) {
                string interactableName = interactionObjects.m_interactable.name;
                Logger.Log($"Interactable: {interactableName}");
            }

            if (interactionObjects.m_TheOriginalHandlePickup != null) {
                string originalName = interactionObjects.m_TheOriginalHandlePickup.name;
                Logger.Log($"Can handle pickup: {originalName}");
            }
        }

        public static void LogPlate() {
            ClientPlate plate = Object.FindObjectOfType<ClientPlate>();
                
            Logger.Log($"Plate location: {Logger.FormatPosition(plate.transform.position)}");

            Logger.Log("Before method call");
                
            Component component = ComponentUtil.GetPlateLocationComponent(plate);
                
            Logger.Log("After method call");

            Logger.Log($"Component name: {component.name}");
        }

        public static void LogCarrying() {
            Logger.Log($"Carrying: {PlayerUtil.GetCarrying(ObjectUtil.GetBotControls())}");
        }

        public static void LogOrders() {
            ArrayList recipes = OrderUtil.GetOrders(ObjectUtil.GetFlowController());
            foreach (var t in recipes) {
                Logger.Log($"Order: {t}");
            }
        }

        public static void LogChefPositions() {
            Vector3 chef1Position = PlayerUtil.GetChefPosition(0);
            Logger.Log($"Chef 1 {Logger.FormatPosition(chef1Position)}");
                
            Vector3 chef2Position = PlayerUtil.GetChefPosition(1);
            Logger.Log($"Chef 2 {Logger.FormatPosition(chef2Position)}");
        }

        public static void LogCookingStations() {
            ClientCookableContainer[] list = Object.FindObjectsOfType<ClientCookableContainer>();
            
            Logger.Log($"Number of objects: {list.Length}");
            
            foreach (var item in list) {
                Logger.Log($"Location: {Logger.FormatPosition(item.transform.position)}");
                
                Logger.Log($"uID of cooking type: {item.GetCookingHandler().AccessCookingType.m_uID}");
            }
        }

        public static void PathFindToPlayer() {
            GridNavSpace gridNavSpace = GameUtils.GetGridNavSpace();
                
            Point2 startPoint = gridNavSpace.GetNavPoint(PlayerUtil.GetChefPosition(1));
            Point2 targetPoint = gridNavSpace.GetNavPoint(PlayerUtil.GetChefPosition(0));
                
            path = gridNavSpace.FindPath(startPoint, targetPoint);

            pathIteration = 0;
                
            currentAction = new MoveAction(ObjectUtil.GetBotControls(), path[0]);

            walkingPath = true;
        }

        public static void Unload() {
            Logger.Log("Unloading...");
            Loader.Unload();
        }
        
        private static bool walkingPath;
        private static List<Vector3> path;
        private static int pathIteration;
        private static Action currentAction;

        public static void Update() {
            if (walkingPath) {
                if (currentAction != null && currentAction.Update()) {
                    currentAction.End();

                    if (pathIteration < path.Count - 1) {
                        currentAction = new MoveAction(ObjectUtil.GetBotControls(), path[++pathIteration]);
                    } else {
                        currentAction = null;
                        walkingPath = false;
                    }
                }
            }
        }
        
    }
}