using System;

namespace AI {

    class PickDropAction : Action {

        private readonly PlayerControls player;
        private String currentlyHolding;

        public PickDropAction(PlayerControls player) {
            this.player = player;
        }

        public override void Initialize() {
            currentlyHolding = PlayerUtil.GetCarrying(player);

            Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);
        }

        public override bool Update() {
            String newHolding = PlayerUtil.GetCarrying(player);
            return !newHolding.Equals(currentlyHolding);
        }

        public override void End() {
            Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);
        }

    }

}
