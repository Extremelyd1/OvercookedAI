namespace AI {

    internal class ProcessIngredientAction : Action {

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

        public override bool Update() {
            return action == null || action.Update();
        }

        public override void End() {
            action?.End();
        }

    }

}
