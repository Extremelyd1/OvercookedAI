namespace AI {

    internal class ChopIngredientAction : Action {

        private readonly PlayerControls player;

        private int state;
        private Action currentAction;

        private readonly ClientWorkstation workstation;

        public ChopIngredientAction(PlayerControls player) {
            this.player = player;
            state = 0;

            workstation = ComponentUtil.GetClosestComponent<ClientWorkstation>(player.transform.position);
            
            currentAction = 
                new PathFindAction(
                    player, 
                    workstation
                );
            
            Logger.Log("ChopIngredientAction instantiated");
        }

        public override bool Update() {
            switch (state) {
                case 0:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 1;
                        currentAction = new PickDropAction(player);
                    }

                    return false;
                case 1:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 2;
                        currentAction = new ChopAction(player, workstation);
                    }
                    
                    return false;
                case 2:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 3;
                        currentAction = new PickDropAction(player);
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

        public override void End() {
            currentAction.End();
        }

    }

}
