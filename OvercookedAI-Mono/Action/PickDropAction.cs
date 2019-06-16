using System;

namespace AI {

    class PickDropAction : Action {

        private static readonly int TRIES = 20;
        
        private readonly PlayerControls player;
        private readonly string currentlyHolding;

        private int retry = TRIES;

        public PickDropAction(PlayerControls player) {
            this.player = player;
            
            currentlyHolding = PlayerUtil.GetCarrying(player);

            Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);
            
            Logger.Log("PickDropAction instantiated");
        }

        public override bool Update() {
            String newHolding = PlayerUtil.GetCarrying(player);

            if (retry == 5) {
                Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);
            } else if (retry == 0) {
                Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);

                retry = TRIES;
                
                return false;
            }
            
            if (newHolding.Equals(currentlyHolding)) {
                retry -= 1;

                return false;
            }
            
            return true;
        }

        public override void End() {
            Keyboard.Get().SendUp(Keyboard.Input.PICK_DROP);
        }

    }

}
