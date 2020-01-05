using System.Collections.Generic;

namespace AI {

    internal class HandleOrderAction : Action {

        private readonly PlayerControls player;
        private readonly OrderData orderData;

        private int state;
        private Action currentAction;
        private Dictionary<ISequentialAction, string> idleActionIngredients;
        private Dictionary<Action, string> pausedActionIngredients;

        private string currentIngredient;

        public HandleOrderAction(PlayerControls player, string order) {
            this.player = player;
            orderData = new OrderData(order);
            state = 0;
            currentIngredient = "";
            orderData.ingredientsDone = new List<string>();
            orderData.plate = null;

            idleActionIngredients = new Dictionary<ISequentialAction, string>();
            pausedActionIngredients = new Dictionary<Action, string>();
            
            Logger.Log("ProcessOrderAction instantiated");
        }

        public bool Update() {
            // Check if we have sequential actions that are no longer idle
            foreach (ISequentialAction sequentialAction in idleActionIngredients.Keys) {
                if (!sequentialAction.IsIdle()) {
                    Logger.Log("Sequential action no longer idle");
                    // Sequential action is not idle anymore, resume it as soon as possible
                    if (currentAction is IPausableAction pausableAction) {
                        if (pausableAction.Pause()) {
                            Logger.Log("Pausing current action");
                            // Action can be paused
                            pausedActionIngredients.Add(pausableAction, currentIngredient);

                            Logger.Log("Resuming non-idle sequential action after PausableAction");
                            currentIngredient = idleActionIngredients[sequentialAction];
                            currentAction = sequentialAction;
                            
                            idleActionIngredients.Remove(sequentialAction);

                            state = 1;
                        } else {
                            Logger.Log("Can not currently pause PausableAction");                            
                        }
                    } else if (currentAction == null) {
                        Logger.Log("Resuming non-idle sequential action after null action");
                        currentIngredient = idleActionIngredients[sequentialAction];
                        currentAction = sequentialAction;

                        idleActionIngredients.Remove(sequentialAction);

                        state = 1;
                    } else {
                        Logger.Log("Current action is not PausableAction");
                    }

                    break;
                }
            }
            
            switch (state) {
                case 0:
                    // If we have any actions that are paused, resume one of them
                    foreach (Action pausedAction in pausedActionIngredients.Keys) {
                        Logger.Log("Found action that was paused, resuming instead of getting new ingredient");
                        currentIngredient = pausedActionIngredients[pausedAction];
                        currentAction = pausedAction;

                        pausedActionIngredients.Remove(pausedAction);

                        state = 1;

                        return false;
                    }
                    
                    // Make a list of ingredients that are we do not need to start making
                    List<string> possibleIngredients = new List<string>(orderData.ingredientsDone);
                    
                    // Add the ingredients of the sequential actions
                    possibleIngredients.AddRange(idleActionIngredients.Values);

                    // Query a possible ingredient to start on
                    currentIngredient = OrderUtil.GetRemainingIngredientFromOrder(
                        orderData.orderName, 
                        possibleIngredients
                    );

                    // If no more ingredients to do
                    if (currentIngredient.Equals("")) {
                        // If we have no idle actions remaining
                        if (idleActionIngredients.Count == 0) {
                            Logger.Log("No new ingredients");
                            // No ingredients left, we are done
                            state = 2;

                            currentAction = new DeliverPlateAction(player, orderData.plate);

                            return false;
                        }

                        state = -1;
                        return false;
                    }
                    
                    Logger.Log($"New ingredient found: {currentIngredient}");
                    
                    state = 1;

                    currentAction = new HandleIngredientAction(player, currentIngredient, orderData);

                    return false;
                case 1:
                    // If we have a sequential action that is idle
                    if (currentAction is ISequentialAction sequentialAction) {
                        if (sequentialAction.IsIdle()) {
                            Logger.Log("Sequential action is idle");
                            List<string> ingredientsDone =
                                new List<string>(orderData.ingredientsDone);
                            ingredientsDone.Add(currentIngredient);

                            string newCurrentIngredient =
                                OrderUtil.GetRemainingIngredientFromOrder(orderData.orderName, ingredientsDone);

                            // If there are other ingredients to work on in the mean time
                            if (!newCurrentIngredient.Equals("")) {
                                Logger.Log($"Other ingredient found to work on: {newCurrentIngredient}");
                                // Store this action and go work on other ingredient
                                idleActionIngredients.Add(sequentialAction, currentIngredient);

                                currentIngredient = newCurrentIngredient;
                                currentAction = new HandleIngredientAction(player, newCurrentIngredient, orderData);
                            }
                        }
                    }
                    
                    if (currentAction.Update()) {
                        currentAction.End();
                        currentAction = null;
                        
                        state = 0;

                        orderData.ingredientsDone.Add(currentIngredient);
                    }
                    
                    return false;
                case 2:
                    if (currentAction.Update()) {
                        currentAction.End();

                        return true;
                    }

                    return false;
                default:
                    return false;
            }
        }

        public void End() {
            currentAction.End();
        }

    }

}
