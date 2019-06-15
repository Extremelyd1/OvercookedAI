using System;
using UnityEngine;

namespace AI {

    class ChopAction : Action {

        private readonly PlayerControls player;
        private readonly ClientWorkstation workstation;

        public ChopAction(PlayerControls player, ClientWorkstation workstation) {
            this.player = player;
            this.workstation = workstation;
        }

        public override void Initialize() {
            Keyboard.Get().SendDown(Keyboard.Input.CHOP_THROW);
        }

        public override bool Update() {
            return StationUtil.HasFinishedChopping(workstation);
        }

        public override void End() {
            Keyboard.Get().SendUp(Keyboard.Input.CHOP_THROW);
        }

    }

}
