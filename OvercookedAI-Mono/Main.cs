using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AI {
    internal class Main : MonoBehaviour {
        private ClientKitchenFlowControllerBase flowController;
        private PlayerControls botPlayer;

        private readonly ArrayList keyDown = new ArrayList();

        private bool executing;

        private string currentOrder = "";

        Action currentAction;

        public void Start() {
            Logger.Clear();
        }
        public void Update() {
            ClientKitchenFlowControllerBase newFlowController = FindObjectOfType<ClientKitchenFlowControllerBase>();
            if (newFlowController != null && (flowController == null || !flowController.Equals(newFlowController))) {
                flowController = newFlowController;
            }

            if (flowController == null) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.J)) {
                if (!keyDown.Contains(KeyCode.J)) {
                    keyDown.Add(KeyCode.J);

                    executing = !executing;

                    if (executing) {
                        botPlayer = FindObjectsOfType<PlayerControls>()[1];
                    }
                }
            } else {
                if (keyDown.Contains(KeyCode.J)) {
                    keyDown.Remove(KeyCode.J);
                }
            }

            if (executing) {
                if (currentOrder.Equals("")) {
                    currentOrder = OrderUtil.GetNewOrder(flowController);
                    Logger.Log("Getting new order");
                } else {
                    bool orderExists = false;
                    foreach (string order in OrderUtil.GetOrders(flowController)) {
                        if (order.Equals(currentOrder)) {
                            orderExists = true;
                            break;
                        }
                    }

                    if (!orderExists) {
                        if (currentAction is CancellableAction cancellableCurrentAction) {
                            cancellableCurrentAction.Cancel();
                        }

                        currentAction = null;

                        currentOrder = OrderUtil.GetNewOrder(flowController);
                        Logger.Log("Getting new order");
                    }
                }
                // We should have an order to work on
                if (currentAction == null) {
                    currentAction = new ProcessOrderAction(botPlayer, currentOrder);                    
                }

                if (currentAction.Update()) {
                    currentAction.End();
                    currentAction = null;
                    currentOrder = "";
                }
            }

            if (Input.GetKeyDown(KeyCode.Y)) {
                currentAction = null;
                currentOrder = "";
            }

            if (Input.GetKeyDown(KeyCode.O)) {
                AttachStation[] attachStations = GameObject.FindObjectsOfType<AttachStation>();

                foreach (AttachStation station in attachStations) {
                    Vector3 pos = station.transform.position;
                    if (pos.x > 10.7 && pos.x < 18.1 && pos.z > -3.7 && pos.z < -3.5) {
                        station.gameObject.Destroy();                        
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.K)) {
                PlayerControls[] playerControlsArray = FindObjectsOfType<PlayerControls>();

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

            if (Input.GetKeyDown(KeyCode.T)) {
                ClientPlate plate = FindObjectOfType<ClientPlate>();
                
                Logger.Log($"Plate location: {Logger.FormatPosition(plate.transform.position)}");

                Logger.Log("Before method call");
                
                Component component = ComponentUtil.GetPlateLocationComponent(plate);
                
                Logger.Log("After method call");

                Logger.Log($"Component name: {component.name}");
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                PlayerControls[] playerControls = FindObjectsOfType<PlayerControls>();

                Logger.Log($"Carrying: {PlayerUtil.GetCarrying(playerControls[1])}");
            }

            if (Input.GetKeyDown(KeyCode.U)) {
                ArrayList recipes = OrderUtil.GetOrders(flowController);
                foreach (var t in recipes) {
                    Logger.Log($"Order: {t}");
                }
            }

            if (Input.GetKeyDown(KeyCode.I)) {
                Vector3 chef1Position = PlayerUtil.GetChefPosition(0);
                Vector3 chef2Position = PlayerUtil.GetChefPosition(1);

                Logger.Log($"Chef 1 {Logger.FormatPosition(chef1Position)}");
                Logger.Log($"Chef 2 {Logger.FormatPosition(chef2Position)}");
            }

            if (Input.GetKeyDown(KeyCode.P) && !temp) {
                temp = true;

                Keyboard.Get().SendDown(Keyboard.Input.MOVE_DOWN);
                Keyboard.Get().SendUp(Keyboard.Input.MOVE_DOWN);
            } else {
                temp = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                Logger.Log("Unloading...");
                Loader.Unload();
            }
        }

        private bool temp;
    }
}