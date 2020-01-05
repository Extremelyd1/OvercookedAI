namespace AI {

    internal class ChopAction : Action {

        private readonly PlayerControls player;
        private readonly ClientWorkstation workstation;

        public ChopAction(PlayerControls player, ClientWorkstation workstation) {
            this.player = player;
            this.workstation = workstation;
            
            Keyboard.Get().SendDown(Keyboard.Input.CHOP_THROW);
            
            Logger.Log("ChopAction instantiated");
        }

        public bool Update() {
            return StationUtil.HasFinishedChopping(workstation);
        }

        public void End() {
            Keyboard.Get().SendUp(Keyboard.Input.CHOP_THROW);
        }

    }

}
