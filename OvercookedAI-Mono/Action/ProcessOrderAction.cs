using System.Collections.Generic;

namespace AI {

    class ProcessOrderAction : Action {

        private readonly PlayerControls player;
        private readonly string order;

        private int state;
        private Action currentAction;

        private string currentIngredient;
        private List<string> ingredientsDone;
        private ClientPlate plate;

        public ProcessOrderAction(PlayerControls player, string order) {
            this.player = player;
            this.order = order;
            state = 0;
            currentIngredient = "";
            ingredientsDone = new List<string>();
            plate = null;
            
            Logger.Log("ProcessOrderAction instantiated");
        }

        public override bool Update() {
            switch (state) {
                case 0:
                    currentIngredient = OrderUtil.GetRemainingIngredientFromOrder(order, ingredientsDone);

                    if (currentIngredient.Equals("")) {
                        // No ingredients left, we are done
                        state = 4;

                        currentAction = new DeliverPlateAction(player, plate);

                        return false;
                    }
                    
                    state = 1;
                    currentAction = new GetRawIngredientAction(player, currentIngredient);

                    return false;
                case 1:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 2;

                        currentAction = new ProcessIngredientAction(player);
                    }
                    
                    return false;
                case 2:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 3;

                        if (plate == null) {
                            plate = ComponentUtil.GetClosestComponent<ClientPlate>(player.transform.position);
                        }
                        currentAction = new PlateHoldingAction(player, plate);
                    }

                    return false;
                case 3:
                    if (currentAction.Update()) {
                        currentAction.End();

                        state = 0;
                        
                        ingredientsDone.Add(currentIngredient);
                    }

                    return false;
                case 4:
                    if (currentAction.Update()) {
                        currentAction.End();

                        return true;
                    }

                    return false;
                default:
                    return false;
            }
        }

        public override void End() {
            currentAction.End();
        }

    }

}
