using System;
using UnityEngine;

namespace AI {

    internal class MoveAction : CancellableAction {

        private static readonly int STUCK_TRIES = 5;
        
        protected PlayerControls player;

        protected Vector3 PlayerPos => player.transform.position;

        protected readonly Vector3 destination;
        protected readonly float xMargin;
        protected readonly float zMargin;
        protected readonly bool stuckStop;

        private float lastX = float.MaxValue;
        private float lastZ = float.MaxValue;
        private float lastRot = float.MaxValue;
        private int stuckCheck = STUCK_TRIES;
        private bool xStuck;
        private bool zStuck;

        public MoveAction(PlayerControls player, Vector3 destination) {
            this.player = player;
            this.destination = destination;
            xMargin = 0.2f;
            zMargin = 0.2f;
            stuckStop = false;
            xStuck = false;
            zStuck = false;

            Logger.Log($"MoveAction instantiated to {Logger.FormatPosition(destination)}, current pos: {Logger.FormatPosition(player.transform.position)}");
        }

        public MoveAction(PlayerControls player, Vector3 destination, float margin) {
            this.player = player;
            this.destination = destination;
            xMargin = margin;
            zMargin = margin;
            stuckStop = false;
        }

        public MoveAction(PlayerControls player, Vector3 destination, bool stuckStop) {
            this.player = player;
            this.destination = destination;
            xMargin = 0.2f;
            zMargin = 0.2f;
            this.stuckStop = stuckStop;
        }

        public MoveAction(PlayerControls player, Vector3 destination, float margin, bool stuckStop) {
            this.player = player;
            this.destination = destination;
            xMargin = margin;
            zMargin = margin;
            this.stuckStop = stuckStop;
        }
        
        public MoveAction(PlayerControls player, Vector3 destination, float xMargin, float zMargin, bool stuckStop) {
            this.player = player;
            this.destination = destination;
            this.xMargin = xMargin;
            this.zMargin = zMargin;
            this.stuckStop = stuckStop;
            
            Logger.Log("MoveAction instantiated");
        }

        public override bool Update() {
            Keyboard.Input release = Keyboard.Input.MOVE_RIGHT;
            Keyboard.Input press = Keyboard.Input.MOVE_LEFT;

            bool xDone = false;
            bool zDone = false;

            if (!xStuck && Math.Abs(PlayerPos.x - destination.x) > xMargin) {
                if (PlayerPos.x < destination.x) {
                    release = Keyboard.Input.MOVE_LEFT;
                    press = Keyboard.Input.MOVE_RIGHT;
                }
                if (Keyboard.Get().IsKeyDown(release)) {
                    Keyboard.Get().SendUp(release);
                }
                Keyboard.Get().SendDown(press);
            } else {
                xDone = true;
                Keyboard.Get().StopXMovement();
            }

            if (!zStuck && Math.Abs(PlayerPos.z - destination.z) > zMargin) {
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
                // TODO: don't rely on movement stuck, but pathfinding and interaction highlights
                // Only check stuck if in range of destination
                if (stuckStop 
                    && Math.Abs(PlayerPos.x - destination.x) < 2 && Math.Abs(PlayerPos.z - destination.z) < 2) {
                    if (stuckCheck == 0) {
//                        Logger.Log($"settings lastX={lastX}, lastZ={lastZ}");
//                        Logger.Log($"pos={Logger.FormatPosition(PlayerPos)}");
                        if (!xDone && Math.Abs(PlayerPos.x - lastX) < 0.0000001) {
//                            Logger.Log("xStuck true");
                            xStuck = true;
                        }
                        if (!zDone && Math.Abs(PlayerPos.z - lastZ) < 0.0000001) {
//                            Logger.Log("zStuck true");
                            zStuck = true;
                        }

                        lastX = PlayerPos.x;
                        lastZ = PlayerPos.z;
                        lastRot = player.transform.rotation.eulerAngles.y;

                        stuckCheck = STUCK_TRIES;
                    } else {
                        stuckCheck -= 1;
                    }
                }
                return false;
            }

            if (xStuck && zStuck) {
                Logger.Log("Movement stuck...");
            } else {
                Logger.Log("Destination reached!");
            }

            return true;
        }

        public override void Cancel() {
            Keyboard.Get().StopXMovement();
            Keyboard.Get().StopZMovement();
        }

        public override void End() {
        }

        public Vector3 GetDestination() {
            return destination;
        }

    }

}
