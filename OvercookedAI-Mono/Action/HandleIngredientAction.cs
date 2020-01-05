namespace AI {
    internal class HandleIngredientAction : ISequentialAction, IPausableAction {

        private PlayerControls player;
        private string ingredient;
        private OrderData orderData;

        private int state;
        private Action currentAction;
        
        public HandleIngredientAction(PlayerControls player, string ingredient, OrderData orderData) {
            this.player = player;
            this.ingredient = ingredient;
            this.orderData = orderData;
            
            state = 0;
        }
        public bool Update() {
            switch (state) {
                case 0:
                    state = 1;
                    currentAction = new GetRawIngredientAction(player, ingredient);

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
                        
                        if (orderData.plate == null) {
                            orderData.plate = ComponentUtil.GetClosestComponent<ClientPlate>(player.transform.position);
                        }

                        currentAction = new PlateHoldingAction(player, orderData.plate);
                    }

                    return false;
                case 3:
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
        }

        public bool IsIdle() {
            if (currentAction is ISequentialAction sequentialAction) {
                return sequentialAction.IsIdle();
            }

            return false;
        }

        public bool Pause() {
            if (currentAction is IPausableAction pausableAction) {
                return pausableAction.Pause();
            }

            return false;
        }
    }
}