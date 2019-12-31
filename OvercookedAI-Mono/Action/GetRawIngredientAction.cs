namespace AI {

    class GetRawIngredientAction : Action {

        private readonly PlayerControls player;

        private int state;
        private Action currentAction;

        public GetRawIngredientAction(PlayerControls player, string ingredient) {
            this.player = player;
            state = 0;
            
            PickupItemSpawner ingredientSpawner = StationUtil.GetSpawnerForItem(ingredient);

            Logger.Log("GetRawIngredientAction instantiated");
            
            currentAction = new PathFindAction(player, ingredientSpawner);
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
