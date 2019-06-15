using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AI {
    internal class Main : MonoBehaviour {

        private ClientKitchenFlowControllerBase flowController;
        private PlayerControls botPlayer;

        private readonly ArrayList keyDown = new ArrayList();

        private bool executing;

        private State state = State.GETTING_ORDER;

        private string currentOrder = "";

        Action currentAction;

        private string currentIngredient = "";

        private int delay = 5;

        private ClientWorkstation currentWorkstation;

        public void Start() {
            //Logger.Log("Create method of injection");
        }

        public void Update() {
            ClientKitchenFlowControllerBase newFlowController = FindObjectOfType<ClientKitchenFlowControllerBase>();
            if (newFlowController != null && (flowController == null || !flowController.Equals(newFlowController))) {
                flowController = newFlowController;
                //Logger.Log("Assigned new flow controller");
            }

            if (flowController == null) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.J)) {
                if (!keyDown.Contains(KeyCode.J)) {
                    keyDown.Add(KeyCode.J);

                    executing = !executing;

                    if (executing) {
                        state = State.GETTING_ORDER;

                        botPlayer = FindObjectsOfType<PlayerControls>()[1];

                        Logger.Log($"Bot player position: {Logger.FormatPosition(botPlayer.transform.position)}");
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

                        currentOrder = OrderUtil.GetNewOrder(flowController);
                    }
                }

                // We should have an order to work on

                // Check whether we have an ingredient that needs to be fetched or processed
                if (currentIngredient.Equals("")) {
                    
                }

                if (state.Equals(State.GETTING_ORDER)) {
                    // If we have no order, fetch the first
                    if (currentOrder.Equals("")) {
                        currentOrder = (string) OrderUtil.GetOrders(flowController)[0];

                        Logger.Log($"Current order: {currentOrder}");
                    }

                    currentIngredient = ItemUtil.GetIngredientsForOrder(currentOrder)[0];

                    PickupItemSpawner ingredientSpawner = StationUtil.GetSpawnerForItem(currentIngredient);
                    Vector3 spawnerPosition = ingredientSpawner.transform.position;

                    if (spawnerPosition.x < 14) {
                        currentAction = new MoveAction(
                            botPlayer, 
                            new Vector3(spawnerPosition.x + 0.9f, spawnerPosition.y, spawnerPosition.z), 
                            true);
                    } else {
                        currentAction = new MoveAction(
                            botPlayer, 
                            new Vector3(spawnerPosition.x - 0.9f, spawnerPosition.y, spawnerPosition.z), 
                            true);
                    }

                    Logger.Log(String.Format("State: {0}", state.ToString()));

                    state = State.MOVING_TO_INGREDIENT;

                    Logger.Log(String.Format("State: {0}", state.ToString()));
                } else if (state.Equals(State.MOVING_TO_INGREDIENT)) {
                    if (currentAction.Update()) {
                        state = State.TAKING_INGREDIENT;
                        Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                        Logger.Log(String.Format("State: {0}", state.ToString()));
                    }
                } else if (state.Equals(State.TAKING_INGREDIENT)) {
                    if (PlayerUtil.IsCarrying(botPlayer)) {
//                    if (PlayerUtil.GetCarrying(botPlayer).Equals(ingredientToGet)) {
                        Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);

                        state = State.MOVING_TO_CHOP;

                        currentWorkstation = ComponentUtil
                            .GetClosestComponent<ClientWorkstation>(PlayerUtil.GetChefPosition(botPlayer));
                        Vector3 workstationPos = currentWorkstation.transform.position;

                        currentAction = new MoveAction(
                            botPlayer, 
                            new Vector3(workstationPos.x, workstationPos.y, workstationPos.z + 0.9f));

                        Logger.Log($"Moving to {Logger.FormatPosition(workstationPos)}");

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.MOVING_TO_CHOP)) {
                    if (currentAction.Update()) {
                        state = State.PLACING_INGREDIENT;
                        Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.PLACING_INGREDIENT)) {
                    if (!PlayerUtil.IsCarrying(botPlayer)) {
                        Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);

                        state = State.CHOPPING_INGREDIENT;
                        Keyboard.Get().SendDown(Keyboard.Input.CHOP_THROW);

                        Logger.Log($"State: {state.ToString()}");
                    } else {
                        Logger.Log($"Carrying {PlayerUtil.GetCarrying(botPlayer)}");
                    }
                } else if (state.Equals(State.CHOPPING_INGREDIENT)) {
                    if (PlayerUtil.HasFinishedChopping(currentWorkstation)) {
                        Keyboard.Get().SendUp(Keyboard.Input.CHOP_THROW);

                        state = State.TAKING_CHOPPED;
                        Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.TAKING_CHOPPED)) {
                    if (PlayerUtil.IsCarrying(botPlayer)) {
                        Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);

                        state = State.FINDING_PLATE;

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.FINDING_PLATE)) {
                    ClientPlate closestPlate = ComponentUtil
                        .GetClosestComponent<ClientPlate>(PlayerUtil.GetChefPosition(botPlayer));
                    Vector3 closestPos = closestPlate.transform.position;

                    if (closestPos.x > 18.5) {
                        currentAction = new MoveAction(
                            botPlayer, 
                            new Vector3(closestPos.x - 0.9f, closestPos.y, closestPos.z), 
                            true);
                    } else {
                        currentAction = new MoveAction(
                            botPlayer, 
                            new Vector3(closestPos.x, closestPos.y, closestPos.z - 0.9f));
                    }

                    state = State.MOVING_TO_PLATE;

                    Logger.Log($"State: {state.ToString()}");
                } else if (state.Equals(State.MOVING_TO_PLATE)) {
                    if (currentAction.Update()) {
                        state = State.PLATING_ORDER;
                        Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.PLATING_ORDER)) {
                    if (!PlayerUtil.IsCarrying(botPlayer)) {
                        Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);

                        state = State.PICKUP_DELAY;

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.PICKUP_DELAY)) {
                    if (delay-- == 0) {
                        delay = 5;

                        state = State.TAKING_PLATE;

                        Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.TAKING_PLATE)) {
                    if (PlayerUtil.IsCarrying(botPlayer)) {
                        Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);

                        state = State.MOVING_TO_DELIVER;

                        ClientPlateStation clientPlateStation = ComponentUtil
                            .GetClosestComponent<ClientPlateStation>(PlayerUtil.GetChefPosition(botPlayer));
                        Vector3 stationPos = clientPlateStation.transform.position;

                        currentAction = new MoveAction(
                            botPlayer,
                            new Vector3(stationPos.x - 0.9f, stationPos.y, stationPos.z));

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.MOVING_TO_DELIVER)) {
                    if (currentAction.Update()) {
                        state = State.DELIVERING;

                        Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                        Logger.Log($"State: {state.ToString()}");
                    }
                } else if (state.Equals(State.DELIVERING)) {
                    if (!PlayerUtil.IsCarrying(botPlayer)) {
                        Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);

                        state = State.GETTING_ORDER;

                        currentOrder = "";

                        Logger.Log($"State: {state.ToString()}");
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                PlayerControls[] playerControls = FindObjectsOfType<PlayerControls>();

                PlayerUtil.GetCarrying(playerControls[1]);
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
        }

        private bool temp;

        public void OnGUI() {

        }

    }

    public enum State {
        GETTING_ORDER,
        MOVING_TO_INGREDIENT,
        TAKING_INGREDIENT,
        MOVING_TO_CHOP,
        PLACING_INGREDIENT,
        CHOPPING_INGREDIENT,
        TAKING_CHOPPED,
        FINDING_PLATE,
        MOVING_TO_PLATE,
        PLATING_ORDER,
        PICKUP_DELAY,
        TAKING_PLATE,
        MOVING_TO_DELIVER,
        DELIVERING
    }

}
