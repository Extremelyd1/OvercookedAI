using System;
using UnityEngine;

namespace AI {

    class MoveAction : CancellableAction {

        private PlayerControls player;

        private Vector3 PlayerPos {
            get {
                return player.transform.position;
            }
        }

        private readonly Vector3 destination;
        private readonly float margin;
        private readonly bool independent;

        public MoveAction(PlayerControls player, Vector3 destination) {
            this.player = player;
            this.destination = destination;
            margin = 0.2f;
            independent = false;
        }

        public MoveAction(PlayerControls player, Vector3 destination, float margin) {
            this.player = player;
            this.destination = destination;
            this.margin = margin;
            independent = false;
        }

        public MoveAction(PlayerControls player, Vector3 destination, bool independent) {
            this.player = player;
            this.destination = destination;
            margin = 0.2f;
            this.independent = independent;
        }

        public MoveAction(PlayerControls player, Vector3 destination, float margin, bool independent) {
            this.player = player;
            this.destination = destination;
            this.margin = margin;
            this.independent = independent;
        }

        public override void Initialize() {
        }

        public override bool Update() {
            if (destination == null) {
                return true;
            }

            Keyboard.Input release = Keyboard.Input.MOVE_RIGHT;
            Keyboard.Input press = Keyboard.Input.MOVE_LEFT;

            bool xDone = false;
            bool zDone = false;

            if (Math.Abs(PlayerPos.x - destination.x) > margin) {
                if (PlayerPos.x < destination.x) {
                    release = Keyboard.Input.MOVE_LEFT;
                    press = Keyboard.Input.MOVE_RIGHT;
                }
                if (Keyboard.Get().IsKeyDown(release)) {
                    Keyboard.Get().SendUp(release);
                }
                Keyboard.Get().SendDown(press);

                if (independent) {
                    return false;
                }
            } else {
                xDone = true;
                Keyboard.Get().StopXMovement();
            }

            if (Math.Abs(PlayerPos.z - destination.z) > margin) {
                if (PlayerPos.z > destination.z) {
                    release = Keyboard.Input.MOVE_UP;
                    press = Keyboard.Input.MOVE_DOWN;
                } else {
                    release = Keyboard.Input.MOVE_DOWN;
                    press = Keyboard.Input.MOVE_UP;
                }
                if (Keyboard.Get().IsKeyDown(release)) {
                    Keyboard.Get().SendUp(release);
                }
                Keyboard.Get().SendDown(press);
            } else {
                zDone = true;
                Keyboard.Get().StopZMovement();
            }

            if (!xDone || !zDone) {
                return false;
            }
            
            Logger.Log("Destination reached!");

            return true;
        }

        public override void Cancel() {
            Keyboard.Get().StopXMovement();
            Keyboard.Get().StopZMovement();
        }

        public override void End() {
        }

    }

}
