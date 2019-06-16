namespace AI {

    class PlateHoldingAction : Action {

        private readonly PlayerControls player;
        private readonly ClientPlate plate;

        private int state;
        private Action currentAction;

        public PlateHoldingAction(PlayerControls player, ClientPlate plate) {
            this.player = player;
            this.plate = plate;
            state = 0;
            
            Logger.Log("PlateHoldingAction instantiated");
            
            currentAction = new MoveTargetAction(player, plate);
        }

        public PlateHoldingAction(PlayerControls player) {
            this.player = player;
            
            plate = ComponentUtil.GetClosestComponent<ClientPlate>(PlayerUtil.GetChefPosition(player));
            state = 0;
            currentAction = new MoveTargetAction(player, plate);
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
