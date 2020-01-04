using System;

namespace AI {

    internal class PickDropAction : Action {

        private static readonly int TRIES = 20;
        
        private readonly PlayerControls player;
        private readonly bool skipHoldingCheck;
        private readonly string currentlyHolding;

        private int retry = TRIES;
        
        public PickDropAction(PlayerControls player) : this(player, false) {
        }

        public PickDropAction(PlayerControls player, bool skipHoldingCheck) {
            this.player = player;
            this.skipHoldingCheck = skipHoldingCheck;
            
            currentlyHolding = PlayerUtil.GetCarrying(player);

            Keyboard.Get().SendDown(Keyboard.Input.PICK_DROP);
            
            Logger.Log("PickDropAction instantiated");
        }

        public override bool Update() {
            if (skipHoldingCheck) {
                retry -= 1;

                if (retry <= 0) {
                    return true;
                }

                return false;
            }
            
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
