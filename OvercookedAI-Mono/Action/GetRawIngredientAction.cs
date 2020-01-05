namespace AI {

    internal class GetRawIngredientAction : IPausableAction {

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

        public bool Update() {
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

        public void End() {
            currentAction.End();
        }

        public bool Pause() {
            if (currentAction is IPausableAction pausableAction) {
                return pausableAction.Pause();
            }

            return false;
        }
    }

}
