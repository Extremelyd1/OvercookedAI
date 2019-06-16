namespace AI {

    class DeliverPlateAction : Action {

        private readonly PlayerControls player;

        private int state;
        private Action currentAction;

        public DeliverPlateAction(PlayerControls player, ClientPlate plate) {
            this.player = player;
            state = 0;
            
            Logger.Log("DeliverPlateAction instantiated");
            
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
                        state = 2;

                        ClientPlateStation deliverStation = ComponentUtil
                            .GetClosestComponent<ClientPlateStation>(player.transform.position);
                        
                        Logger.Log($"Current pos: {Logger.FormatPosition(player.transform.position)}, " +
                                   $"station pos: {Logger.FormatPosition(deliverStation.transform.position)}");
                        
                        currentAction = new MoveTargetAction(player, deliverStation);
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
