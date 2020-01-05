namespace AI {

    internal class ProcessIngredientAction : ISequentialAction, IPausableAction {

        private readonly Action action;

        public ProcessIngredientAction(PlayerControls player) {
            string carrying = PlayerUtil.GetCarrying(player);

            Logger.Log("ProcessIngredientAction instantiated");
            
            switch (carrying) {
                case "SushiFish":
                case "SushiPrawn":
                    action = new ChopIngredientAction(player);
                    break;
                case "SushiRice":
                    action = new CookIngredientAction(player);
                    break;
                case "Seaweed":
                    action = null;
                    break;
            }
        }

        public bool Update() {
            return action == null || action.Update();
        }

        public void End() {
            action?.End();
        }

        public bool IsIdle() {
            if (action is ISequentialAction sequentialAction) {
                return sequentialAction.IsIdle();
            }

            return false;
        }

        public bool Pause() {
            if (action is IPausableAction pausableAction) {
                return pausableAction.Pause();
            }

            return false;
        }
    }

}
